using Hangfire;
using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Services.Interfaces;
using MangoParser.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace MangoParser.Services.Realizations
{
    public class MangaLibParsingService : IMangaLibParsingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MangoParserSettings _mangoParserSettings;
        private readonly IMangaService _mangaService;

        public MangaLibParsingService(IHttpClientFactory httpClientFactory, IOptions<MangoParserSettings> options, IMangaService mangaService)
        {
            _httpClientFactory = httpClientFactory;
            _mangoParserSettings = options.Value;
            _mangaService = mangaService;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = [60])]
        public async Task ParseAllMangas()
        {
            int i = 1;
            HttpStatusCode statusCode;

            do
            {
                statusCode = await ParseMangaPageAsync(i, "asc");
                await Task.Delay(_mangoParserSettings.TimeoutBetweenRequestsMilliseconds);

                i++;
            }
            while (statusCode != HttpStatusCode.NotFound);
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = [60])]
        public async Task ParseMangaWithPagesCountAsync()
        {
            for (int i = 1; i <= _mangoParserSettings.ParsingPageCount; i++)
            {
                await ParseMangaPageAsync(i, "desc");
                await Task.Delay(_mangoParserSettings.TimeoutBetweenRequestsMilliseconds);
            }
            for (int i = 1; i <= _mangoParserSettings.ParsingPageCount; i++)
            {
                await ParseMangaPageAsync(i, "asc");
                await Task.Delay(_mangoParserSettings.TimeoutBetweenRequestsMilliseconds);
            }
        }

        private async Task<HttpStatusCode> ParseMangaPageAsync(int pageNum, string sortType)
        {
            using var httpClient = _httpClientFactory.CreateClient(_mangoParserSettings.ClientName);

            var requestUrl = $"{_mangoParserSettings.BaseUrl}" +
                $"{_mangoParserSettings.MangaPageEndPoint}"
                .Replace("{rateMin}", _mangoParserSettings.RateMin)
                .Replace("{pageNum}", pageNum.ToString())
                .Replace("{siteId}", _mangoParserSettings.SiteId)
                .Replace("{sortBy}", _mangoParserSettings.SortBy)
                .Replace("{sortType}", sortType);

            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
                return response.StatusCode;

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var slugUrls = doc.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(x => x.GetProperty("slug_url").GetString())
                .ToList();

            foreach (var slugUrl in slugUrls)
            {
                await ParseMangaItemAsync(slugUrl!);
                await Task.Delay(_mangoParserSettings.TimeoutBetweenRequestsMilliseconds - 9000);
            }

            return response.StatusCode;
        }

        private async Task ParseMangaItemAsync(string slugUrl)
        {
            using var httpClient = _httpClientFactory.CreateClient(_mangoParserSettings.ClientName);

            var requestUrl = $"{_mangoParserSettings.BaseUrl}" +
                $"{_mangoParserSettings.MangaItemEndPoint}"
                .Replace("{slugUrl}", slugUrl);

            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
                return;

            var data = await response.Content.ReadFromJsonAsync<ApiResponse<MangaFromParsingDTO>>();

            if (data is null)
                return;

            await _mangaService.CreateOrUpdateMangaFromParser(data.Data);
        }

    }
}
