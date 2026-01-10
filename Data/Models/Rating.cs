namespace MangoParser.Data.Models
{
    public class Rating
    {
        public int MangaId { get; set; }
        public decimal AverageRating { get; set; }
        public int VotesCount { get; set; }

        public virtual Manga Manga { get; set; } = null!;
    }
}
