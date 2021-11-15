using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseByGuidQuery : IQuery<BookcaseDto>
    {
        public Guid BookcaseGuid { get; }

        public BookcaseByGuidQuery(Guid bookcaseGuid)
        {
            BookcaseGuid = bookcaseGuid;
        }
    }
}