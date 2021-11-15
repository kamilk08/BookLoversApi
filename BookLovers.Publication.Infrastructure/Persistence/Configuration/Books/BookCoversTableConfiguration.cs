using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class BookCoversTableConfiguration : EntityTypeConfiguration<BookCoverReadModel>
    {
        public BookCoversTableConfiguration()
        {
            this.ToTable("BookCovers");

            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.CoverUrl).IsRequired().HasColumnOrder(2);

            this.Property(p => p.FileName).IsRequired().HasColumnOrder(3);

            this.Property(p => p.MimeType).IsRequired().HasColumnOrder(4);

            this.Property(p => p.BookGuid).IsRequired();
        }
    }
}