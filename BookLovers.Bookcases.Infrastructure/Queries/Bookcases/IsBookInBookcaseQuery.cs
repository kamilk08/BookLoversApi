using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class IsBookInBookcaseQuery : IQuery<bool>
    {
        public Guid ReaderGuid { get; set; }

        public Guid BookGuid { get; set; }

        public IsBookInBookcaseQuery()
        {
        }

        public IsBookInBookcaseQuery(Guid readerGuid, Guid bookGuid)
        {
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
        }
    }
}