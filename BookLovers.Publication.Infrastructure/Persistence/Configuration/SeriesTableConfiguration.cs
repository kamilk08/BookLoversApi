using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration
{
    internal class SeriesTableConfiguration : EntityTypeConfiguration<SeriesReadModel>
    {
        public SeriesTableConfiguration()
        {
            this.ToTable("Series");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired()
                .HasColumnName("Guid").HasColumnOrder(2)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_UserName")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Name)
                .IsRequired().HasColumnName("Name")
                .HasColumnOrder(3).HasMaxLength(byte.MaxValue);

            this.Property(p => p.Status).IsRequired()
                .HasColumnName("Status").HasColumnOrder(4);
        }
    }
}