using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderAddedAuthor : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public int AuthorId { get; private set; }

        public DateTime AddedAt { get; private set; }

        private ReaderAddedAuthor()
        {
        }

        [JsonConstructor]
        protected ReaderAddedAuthor(
            Guid guid,
            Guid aggregateGuid,
            Guid authorGuid,
            int authorId,
            DateTime addedAt)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            AuthorGuid = authorGuid;
            AuthorId = authorId;
            AddedAt = addedAt;
        }

        private ReaderAddedAuthor(Guid aggregateGuid, Guid authorGuid, int authorId, DateTime addedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            AuthorGuid = authorGuid;
            AuthorId = authorId;
            AddedAt = addedAt;
        }

        public static ReaderAddedAuthor Initialize()
        {
            return new ReaderAddedAuthor();
        }

        public ReaderAddedAuthor WithAggregate(Guid aggregateGuid)
        {
            return new ReaderAddedAuthor(aggregateGuid, AuthorGuid, AuthorId, AddedAt);
        }

        public ReaderAddedAuthor WithAuthor(Guid authorGuid, int authorId)
        {
            return new ReaderAddedAuthor(AggregateGuid, authorGuid, authorId, AddedAt);
        }

        public ReaderAddedAuthor WithAddedAt(DateTime addedAt)
        {
            return new ReaderAddedAuthor(AggregateGuid, AuthorGuid, AuthorId, addedAt);
        }
    }
}