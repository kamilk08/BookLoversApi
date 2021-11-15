using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class MultipleBooksByIdQuery : IQuery<List<BookDto>>
    {
        public List<int> BookIds { get; set; }

        public MultipleBooksByIdQuery()
        {
            this.BookIds = this.BookIds ?? new List<int>();
        }

        public MultipleBooksByIdQuery(List<int> bookIds)
        {
            this.BookIds = bookIds;
        }
    }
}