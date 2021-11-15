using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class PublisherCycleTableConfiguration : EntityTypeConfiguration<PublisherCycleReadModel>
    {
        public PublisherCycleTableConfiguration()
        {
            this.ToTable("PublisherCycles");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.PublisherCycleGuid).IsRequired();

            this.Property(p => p.PublisherCycleId)
                .IsRequired().HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_PublisherCycleId")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Status).IsRequired();

            this.HasMany(p => p.Books);
        }
    }
}