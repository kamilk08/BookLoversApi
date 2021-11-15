using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class BookReviewsTableConfiguration : EntityTypeConfiguration<ReviewReadModel>
    {
        public BookReviewsTableConfiguration()
        {
            this.ToTable("Reviews");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).HasColumnOrder(2).IsRequired();

            this.HasRequired(p => p.Reader);
        }
    }
}