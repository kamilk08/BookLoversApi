using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries
{
    public class BookByGuidQuery : IQuery<BookDto>
    {
        public Guid BookGuid { get; }

        public BookByGuidQuery(Guid bookGuid)
        {
            BookGuid = bookGuid;
        }
    }
}