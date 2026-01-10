using Hangfire;
using Hangfire.PostgreSql;
using MangoParser.Data.DB;
using MangoParser.Services.Interfaces;
using MangoParser.Services.Realizations;
using MangoParser.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MangoParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var mangoParserSettings = config.GetSection(nameof(MangoParserSettings));

            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("../Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog(logger);

            builder.Services.Configure<MangoParserSettings>(mangoParserSettings);

            builder.Services
                .AddSwaggerGen()
                .AddDbContextFactory<MangoParserDbContext>(options =>
                    options.UseNpgsql(connectionString))
                .AddScoped<IMangaLibParsingService, MangaLibParsingService>()
                .AddScoped<IMangaService, MangaService>()
                .AddScoped<ITagService, TagService>()
                .AddScoped<IGenreService, GenreService>();

            builder.Services.AddHangfire(c => c
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(config.GetConnectionString("DefaultConnection")),
                    new PostgreSqlStorageOptions
                    {
                        InvisibilityTimeout = TimeSpan.FromMinutes(90)
                    }));
            builder.Services.AddHangfireServer();

            builder.Services.AddHttpClient(mangoParserSettings["ClientName"]!, client =>
            {
                client.DefaultRequestHeaders.Add("origin", mangoParserSettings["Origin"]);
                client.DefaultRequestHeaders.Add("referer", mangoParserSettings["Referer"]);
                client.DefaultRequestHeaders.Add("site-id", mangoParserSettings["SiteId"]);
                client.DefaultRequestHeaders.Add("user-agent", mangoParserSettings["UserAgent"]);
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    DashboardTitle = "My Jobs Dashboard",
                    StatsPollingInterval = 5000,
                    AppPath = "/"
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Lifetime.ApplicationStarted.Register(() =>
                RecurringJob.AddOrUpdate<MangaLibParsingService>(
                    "full-parse",
                    mp => mp.ParseAllMangas(),
                    Cron.Daily(2),
                     new RecurringJobOptions { TimeZone = TimeZoneInfo.Local })
            );

            app.Run();
        }
    }
}
