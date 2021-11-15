using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class BookByIsbnQuery : IQuery<BookDto>
    {
        public string Isbn { get; set; }

        public BookByIsbnQuery()
        {
        }

        public BookByIsbnQuery(string isbn)
        {
            this.Isbn = isbn;
        }
    }
}