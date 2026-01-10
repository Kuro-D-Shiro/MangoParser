namespace MangoParser.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}
