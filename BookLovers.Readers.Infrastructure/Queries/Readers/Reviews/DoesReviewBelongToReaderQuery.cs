using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Reviews
{
    public class DoesReviewBelongToReaderQuery : IQuery<bool>
    {
        public Guid ReviewGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public DoesReviewBelongToReaderQuery(Guid reviewGuid, Guid readerGuid)
        {
            this.ReviewGuid = reviewGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}