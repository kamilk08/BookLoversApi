using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class CoverTypesTableConfiguration : EntityTypeConfiguration<CoverTypeReadModel>
    {
        public CoverTypesTableConfiguration()
        {
            this.ToTable("CoverTypes");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.CoverType).HasColumnOrder(2)
                .IsRequired().HasMaxLength(byte.MaxValue)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_CoverType")
                    {
                        IsUnique = true
                    }));
        }
    }
}