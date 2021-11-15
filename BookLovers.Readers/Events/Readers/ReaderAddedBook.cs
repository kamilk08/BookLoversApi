using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderAddedBook : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        public DateTime AddedAt { get; private set; }

        private ReaderAddedBook()
        {
        }

        [JsonConstructor]
        protected ReaderAddedBook(
            Guid guid,
            Guid aggregateGuid,
            Guid bookGuid,
            int bookId,
            DateTime addedAt)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            BookGuid = bookGuid;
            BookId = bookId;
            AddedAt = addedAt;
        }

        private ReaderAddedBook(Guid aggregateGuid, Guid bookGuid, int bookId, DateTime addedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookGuid = bookGuid;
            BookId = bookId;
            AddedAt = addedAt;
        }

        public static ReaderAddedBook Initialize()
        {
            return new ReaderAddedBook();
        }

        public ReaderAddedBook WithAggregate(Guid aggregateGuid)
        {
            return new ReaderAddedBook(aggregateGuid, BookGuid, BookId, AddedAt);
        }

        public ReaderAddedBook WithBook(Guid bookGuid, int bookId)
        {
            return new ReaderAddedBook(AggregateGuid, bookGuid, bookId, AddedAt);
        }

        public ReaderAddedBook WithAddedAt(DateTime addedAt)
        {
            return new ReaderAddedBook(AggregateGuid, BookGuid, BookId, addedAt);
        }
    }
}