namespace MangoParser.Services.Interfaces
{
    public interface IMangaLibParsingService
    {
        public Task ParseMangaWithPagesCountAsync();
        public Task ParseAllMangas();
    }
}
