namespace MangoParser.Data.DTO.ParserDTOs
{
    public class MangaFromParsingDTO
    {
        public string Name { get; set; }
        public string Eng_Name { get; set; }
        public string Rus_Name { get; set; }
        public string Summary { get; set; }

        public RatingFromParsingDTO Rating { get; set; }

        public List<TagFromParsingDTO> Tags { get; set; }
        public List<GenreFromParsingDTO> Genres { get; set; }
    }
}
