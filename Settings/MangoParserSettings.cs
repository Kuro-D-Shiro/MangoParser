namespace MangoParser.Settings
{
    public class MangoParserSettings
    {
        public string Token { get; set; }
        public string BaseUrl { get; set; }
        public string ClientName { get; set; }
        public int TimeoutBetweenRequestsMilliseconds { get; set; }
        public string MangaPageEndPoint { get; set; }
        public string MangaItemEndPoint { get; set; }
        public string SortBy { get; set; } 
        public string RateMin { get; set; }
        public int ParsingPageCount { get; set; }
        public string SiteId { get; set; }
    }
}
