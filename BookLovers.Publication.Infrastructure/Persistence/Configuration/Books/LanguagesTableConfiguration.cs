using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class LanguagesTableConfiguration : EntityTypeConfiguration<LanguageReadModel>
    {
        public LanguagesTableConfiguration()
        {
            this.ToTable("Languages");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Name).IsRequired()
                .HasColumnName("Name").HasColumnOrder(2)
                .HasMaxLength(byte.MaxValue)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Language")
                    {
                        IsUnique = true
                    }));
        }
    }
}