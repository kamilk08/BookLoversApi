using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class FavouriteAuthorRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private FavouriteAuthorRemoved()
        {
        }

        [JsonConstructor]
        protected FavouriteAuthorRemoved(
            Guid guid,
            Guid aggregateGuid,
            Guid authorGuid,
            Guid readerGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            AuthorGuid = authorGuid;
            ReaderGuid = readerGuid;
        }

        public FavouriteAuthorRemoved(Guid aggregateGuid, Guid authorGuid, Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            AuthorGuid = authorGuid;
            ReaderGuid = readerGuid;
        }
    }
}