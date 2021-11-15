using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration
{
    internal class PublishersTableConfiguration : EntityTypeConfiguration<PublisherReadModel>
    {
        public PublishersTableConfiguration()
        {
            this.ToTable("Publishers");

            this.HasKey(k => k.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired()
                .HasColumnName("Guid").HasColumnOrder(2)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Publisher)
                .IsRequired().HasColumnName("Publisher").HasColumnOrder(3);

            this.HasMany(p => p.Cycles)
                .WithOptional(p => p.Publisher).WillCascadeOnDelete(true);

            this.Property(p => p.Status).IsRequired()
                .HasColumnName("Status").HasColumnOrder(4);
        }
    }
}