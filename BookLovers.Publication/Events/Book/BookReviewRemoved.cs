using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookReviewRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private BookReviewRemoved()
        {
        }

        public BookReviewRemoved(Guid aggregateGuid, Guid reviewGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReviewGuid = reviewGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}