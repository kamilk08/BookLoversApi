using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class BooksTableConfiguration : EntityTypeConfiguration<BookReadModel>
    {
        public BooksTableConfiguration()
        {
            this.ToTable("Books");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.BookGuid).IsRequired();

            this.Property(p => p.BookId)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_BookId")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Status).IsRequired();

            this.HasMany(p => p.Ratings);
        }
    }
}