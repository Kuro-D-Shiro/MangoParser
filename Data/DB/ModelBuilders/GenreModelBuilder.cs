using MangoParser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Data.DB.ModelBuilders
{
    public static class GenreModelBuilder
    {
        public static ModelBuilder AddGenres(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genres");

                entity.HasKey(x => x.Id).HasName("genre_id_pk");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasColumnName("name");

                entity.HasMany(e => e.Mangas).WithMany(m => m.Genres)
                    .UsingEntity<Dictionary<string, object>>("manga_genre_ref",
                        l => l.HasOne<Manga>().WithMany().HasForeignKey("manga_id"),
                        r => r.HasOne<Genre>().WithMany().HasForeignKey("genre_id"),
                        j =>
                        {
                            j.HasKey("manga_id", "genre_id").HasName("manga_ganre_pk");
                            j.ToTable("manga_ganre_ref");
                            j.HasIndex(["genre_id", "manga_id"], "IX_manga_ganre_ref");
                        });
            });

            return modelBuilder;
        }
    }
}
