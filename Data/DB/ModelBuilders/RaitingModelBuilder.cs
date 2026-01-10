using MangoParser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Data.DB.ModelBuilders
{
    public static class RaitingModelBuilder
    {
        public static ModelBuilder AddRatings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("ratings");

                entity.HasKey(x => x.MangaId).HasName("rating_id_pk");

                entity.Property(x => x.MangaId).HasColumnName("manga_id");
                entity.Property(x => x.AverageRating).HasColumnName("average_rating");
                entity.Property(x => x.VotesCount).HasColumnName("votes_count");

                entity.HasOne(x => x.Manga)
                    .WithOne(x => x.Rating)
                    .HasForeignKey<Rating>(x => x.MangaId)
                    .HasConstraintName("rating_manga_id_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            return modelBuilder;
        }
    }
}
