using MangoParser.Data.Models;

namespace MangoParser.Data.DTO.ResponseDTOs
{
    public class RatingWithoutIdDTO
    {
        public RatingWithoutIdDTO(Rating raring)
        {
            AverageRating = raring.AverageRating;
            VotesCount = raring.VotesCount;
        }

        public decimal AverageRating { get; set; }
        public int VotesCount { get; set; }
    }
}
