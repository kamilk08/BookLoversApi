using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
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