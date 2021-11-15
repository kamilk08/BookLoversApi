using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderAuthorRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        private ReaderAuthorRemoved()
        {
        }

        [JsonConstructor]
        protected ReaderAuthorRemoved(Guid guid, Guid aggregateGuid, Guid readerGuid, Guid authorGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
        }

        public ReaderAuthorRemoved(Guid aggregateGuid, Guid readerGuid, Guid authorGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
        }
    }
}