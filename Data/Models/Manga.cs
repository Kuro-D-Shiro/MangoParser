namespace MangoParser.Data.Models
{
    public class Manga
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string EnName { get; set; } = null!;
        public string RuName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime LastCheckout { get; set; }

        public virtual Rating Rating { get; set; } = null!;
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
