using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class BooksTableConfiguration : EntityTypeConfiguration<BookReadModel>
    {
        public BooksTableConfiguration()
        {
            ToTable("Books");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.BookGuid).IsRequired()
                .HasColumnOrder(2).HasColumnName("BookGuid");

            Property(p => p.Status).IsRequired()
                .HasColumnOrder(3)
                .HasColumnName("Status");

            Property(p => p.BookId).IsRequired()
                .HasColumnOrder(4)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_BookId")
                    {
                        IsUnique = true
                    }));
        }
    }
}