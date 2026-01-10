namespace MangoParser.Data.DTO.ResponseDTOs
{
    public class TrainingDataSetDTO
    {
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public RatingWithoutIdDTO Rating { get; set; }
    }
}
