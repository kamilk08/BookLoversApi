using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class BooksTableConfiguration : EntityTypeConfiguration<BookReadModel>
    {
        public BooksTableConfiguration()
        {
            this.ToTable("Books");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.BookGuid).HasColumnOrder(2);

            this.Property(p => p.BookId).HasColumnOrder(3)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_BookId")
                    {
                        IsUnique = true
                    }));
        }
    }
}