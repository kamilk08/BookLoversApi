using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.BookReaders
{
    public class BookReaderSuspended : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int BookReaderId { get; private set; }

        public int Status { get; private set; }

        private BookReaderSuspended()
        {
        }

        public BookReaderSuspended(Guid aggregateGuid, int bookReaderId)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.BookReaderId = bookReaderId;
            this.Status = AggregateStatus.Archived.Value;
        }
    }
}