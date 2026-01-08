using MangoParser.Data.DB.ModelBuilders;
using MangoParser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Data.DB
{
    public class MangoParserDbContext : DbContext
    {
        public DbSet<Manga> Mangas { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;


        public MangoParserDbContext(DbContextOptions<MangoParserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .AddMangas()
                .AddRatings()
                .AddGenres()
                .AddTags();
        }
    }
}
