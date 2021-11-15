using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class FavouriteBookRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private FavouriteBookRemoved()
        {
        }

        [JsonConstructor]
        protected FavouriteBookRemoved(Guid guid, Guid aggregateGuid, Guid readerGuid, Guid bookGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
        }

        public FavouriteBookRemoved(Guid aggregateGuid, Guid readerGuid, Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
        }
    }
}