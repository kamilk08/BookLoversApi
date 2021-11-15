using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class PublisherTableConfiguration : EntityTypeConfiguration<PublisherReadModel>
    {
        public PublisherTableConfiguration()
        {
            this.ToTable("Publishers");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.Status).IsRequired();

            this.Property(p => p.PublisherGuid).IsRequired();

            this.Property(p => p.PublisherId)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_PublisherId")
                    {
                        IsUnique = true
                    }));

            this.HasMany(p => p.Books);

            this.HasMany(p => p.PublisherCycles);
        }
    }
}