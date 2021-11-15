using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Books
{
    public class BookGroupedRatingsByIdQuery : IQuery<BookGroupedRatingsDto>
    {
        public int BookId { get; set; }

        public BookGroupedRatingsByIdQuery()
        {
        }

        public BookGroupedRatingsByIdQuery(int bookId)
        {
            this.BookId = bookId;
        }
    }
}