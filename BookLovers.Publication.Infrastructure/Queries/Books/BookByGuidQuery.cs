using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class BookByGuidQuery : IQuery<BookDto>
    {
        public Guid BookGuid { get; set; }

        public BookByGuidQuery()
        {
        }

        public BookByGuidQuery(Guid bookGuid)
        {
            this.BookGuid = bookGuid;
        }
    }
}