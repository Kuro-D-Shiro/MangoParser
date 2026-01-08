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

                entity.HasKey(x => x.Id).HasName("rating_id_pk");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.AvarageRating).HasColumnName("avarage_rating");
                entity.Property(x => x.VotesCount).HasColumnName("votes_count");

                entity.HasOne(x => x.Manga)
                    .WithOne(x => x.Rating)
                    .HasForeignKey<Rating>(x => x.Id)
                    .HasConstraintName("rating_manga_id_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            return modelBuilder;
        }
    }
}
