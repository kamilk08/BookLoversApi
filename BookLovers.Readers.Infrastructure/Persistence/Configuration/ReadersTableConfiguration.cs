using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class ReadersTableConfiguration : EntityTypeConfiguration<ReaderReadModel>
    {
        public ReadersTableConfiguration()
        {
            this.ToTable("Readers");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired().HasColumnOrder(2).HasColumnName("Guid");

            this.Property(p => p.ReaderId).IsRequired().HasColumnOrder(3);

            this.Property(p => p.UserName).IsRequired()
                .HasColumnName("Username").HasColumnOrder(4)
                .HasMaxLength(255)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_UserName")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.AddedResourcesCount).HasColumnOrder(6);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(7);

            this.HasMany(p => p.Followers).WithRequired(p => p.Followed)
                .WillCascadeOnDelete(true);
        }
    }
}