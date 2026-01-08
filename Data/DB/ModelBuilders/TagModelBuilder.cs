using MangoParser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Data.DB.ModelBuilders
{
    public static class TagModelBuilder
    {
        public static ModelBuilder AddTags(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags");

                entity.HasKey(x => x.Id).HasName("tag_id_pk");

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasColumnName("name");

                entity.HasMany(e => e.Mangas).WithMany(m => m.Tags)
                    .UsingEntity<Dictionary<string, object>>("manga_tag_ref",
                        l => l.HasOne<Manga>().WithMany().HasForeignKey("manga_id"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("tag_id"),
                        j =>
                        {
                            j.HasKey("manga_id", "tag_id").HasName("manga_tag_pk");
                            j.ToTable("manga_tag_ref");
                            j.HasIndex(["manga_id", "tag_id"], "IX_manga_tag_ref");
                        });
            });

            return modelBuilder;
        }
    }
}
