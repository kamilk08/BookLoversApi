using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Books
{
    public class BookByIdQuery : IQuery<BookDto>
    {
        public int BookId { get; set; }

        public BookByIdQuery()
        {
        }

        public BookByIdQuery(int bookId)
        {
            this.BookId = bookId;
        }
    }
}