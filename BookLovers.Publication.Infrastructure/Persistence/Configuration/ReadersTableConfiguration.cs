using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration
{
    internal class ReadersTableConfiguration : EntityTypeConfiguration<ReaderReadModel>
    {
        public ReadersTableConfiguration()
        {
            this.ToTable("Readers");

            this.HasKey(p => p.Id);

            this.Property(p => p.ReaderId).HasColumnOrder(2)
                .IsRequired().HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_ReaderId")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.ReaderGuid)
                .HasColumnOrder(3).IsRequired();

            this.Property(p => p.Status)
                .HasColumnOrder(4).IsRequired();
        }
    }
}