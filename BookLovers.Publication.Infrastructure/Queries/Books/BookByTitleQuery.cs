using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class BookByTitleQuery : IQuery<BookDto>
    {
        public string Title { get; set; }

        public BookByTitleQuery()
        {
        }

        public BookByTitleQuery(string title)
        {
            this.Title = title;
        }
    }
}