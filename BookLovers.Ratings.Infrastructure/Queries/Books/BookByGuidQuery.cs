using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Books
{
    public class BookByGuidQuery : IQuery<BookDto>
    {
        public Guid BookGuid { get; }

        public BookByGuidQuery(Guid bookGuid)
        {
            this.BookGuid = bookGuid;
        }
    }
}