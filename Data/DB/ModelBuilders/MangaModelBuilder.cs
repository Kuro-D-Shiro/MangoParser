using MangoParser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Data.DB.ModelBuilders
{
    public static class MangaModelBuilder
    {
        public static ModelBuilder AddMangas(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manga>(entity =>
            {
                entity.ToTable("mangas");

                entity.HasKey(x => x.Id).HasName("manga_id_pk");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasColumnName("name");
                entity.Property(x => x.EnName).HasColumnName("en_name");
                entity.Property(x => x.RuName).HasColumnName("ru_name");
                entity.Property(x => x.Description).HasColumnName("description");
                entity.Property(x => x.LastCheckout).HasColumnType("timestamp with time zone")
                    .HasPrecision(0).HasColumnName("last_checkout");
            });

            return modelBuilder;
        }
    }
}
