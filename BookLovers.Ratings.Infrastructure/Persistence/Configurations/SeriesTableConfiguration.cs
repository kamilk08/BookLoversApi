using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class SeriesTableConfiguration : EntityTypeConfiguration<SeriesReadModel>
    {
        public SeriesTableConfiguration()
        {
            this.ToTable("Series");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.Status).IsRequired();

            this.Property(p => p.SeriesGuid).IsRequired();

            this.Property(p => p.SeriesId)
                .IsRequired().HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_SeriesId")
                    {
                        IsUnique = true
                    }));

            this.HasMany(p => p.Books);
        }
    }
}