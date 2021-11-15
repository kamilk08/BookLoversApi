using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Events.BookReaders;

namespace BookLovers.Publication.Domain.BookReaders
{
    public class BookReader :
        EventSourcedAggregateRoot,
        IHandle<BookReaderCreated>,
        IHandle<BookReaderSuspended>
    {
        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        private BookReader()
        {
        }

        public BookReader(Guid aggregateGuid, Guid readerGuid, int readerId)
        {
            this.Guid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
            this.ApplyChange(new BookReaderCreated(aggregateGuid, readerGuid, readerId));
        }

        void IHandle<BookReaderCreated>.Handle(BookReaderCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this.ReaderGuid = @event.BookReaderGuid;
            this.ReaderId = @event.BookReaderId;
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<BookReaderSuspended>.Handle(BookReaderSuspended @event)
        {
            this.AggregateStatus = AggregateStatus.Archived;
        }
    }
}