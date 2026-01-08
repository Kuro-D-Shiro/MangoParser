namespace MangoParser.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public decimal AvarageRating { get; set; }
        public int VotesCount { get; set; }

        public virtual Manga Manga { get; set; } = null!;
    }
}
