using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.BookReaders
{
    public class BookReaderCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookReaderGuid { get; private set; }

        public int BookReaderId { get; private set; }

        public int Status { get; private set; }

        private BookReaderCreated()
        {
        }

        public BookReaderCreated(Guid aggregateGuid, Guid bookReaderGuid, int bookReaderId)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.BookReaderGuid = bookReaderGuid;
            this.BookReaderId = bookReaderId;
            this.Status = AggregateStatus.Active.Value;
        }
    }
}