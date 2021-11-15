using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseByGuidOptionsQuery : IQuery<BookcaseOptionsDto>
    {
        public Guid BookcaseGuid { get; }

        public BookcaseByGuidOptionsQuery(Guid bookcaseGuid)
        {
            BookcaseGuid = bookcaseGuid;
        }
    }
}