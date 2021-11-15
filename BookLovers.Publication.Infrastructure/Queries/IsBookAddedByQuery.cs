using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class IsBookAddedByQuery : IQuery<bool>
    {
        public Guid ReaderGuid { get; }

        public Guid BookGuid { get; }

        public IsBookAddedByQuery(Guid readerGuid, Guid bookGuid)
        {
            this.ReaderGuid = readerGuid;
            this.BookGuid = bookGuid;
        }
    }
}