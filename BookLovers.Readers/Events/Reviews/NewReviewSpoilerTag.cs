using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class NewReviewSpoilerTag : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private NewReviewSpoilerTag()
        {
        }

        [JsonConstructor]
        protected NewReviewSpoilerTag(Guid guid, Guid aggregateGuid, Guid readerGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
        }

        public NewReviewSpoilerTag(Guid aggregateGuid, Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
        }
    }
}