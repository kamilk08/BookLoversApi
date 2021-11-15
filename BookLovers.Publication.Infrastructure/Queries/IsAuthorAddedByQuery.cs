using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class IsAuthorAddedByQuery : IQuery<bool>
    {
        public Guid ReaderGuid { get; }

        public Guid AuthorGuid { get; }

        public IsAuthorAddedByQuery(Guid readerGuid, Guid authorGuid)
        {
            this.ReaderGuid = readerGuid;
            this.AuthorGuid = authorGuid;
        }
    }
}