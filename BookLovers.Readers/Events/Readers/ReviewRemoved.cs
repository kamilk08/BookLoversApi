using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReviewRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        private ReviewRemoved()
        {
        }

        [JsonConstructor]
        protected ReviewRemoved(Guid guid, Guid aggregateGuid, Guid readerGuid, Guid reviewGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            ReviewGuid = reviewGuid;
        }

        public ReviewRemoved(Guid aggregateGuid, Guid readerGuid, Guid reviewGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            ReviewGuid = reviewGuid;
        }
    }
}