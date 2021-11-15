using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewSpoilerTagRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int SpoilerTagsCount { get; private set; }

        private ReviewSpoilerTagRemoved()
        {
        }

        [JsonConstructor]
        protected ReviewSpoilerTagRemoved(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            int spoilerTagsCount)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            SpoilerTagsCount = spoilerTagsCount;
        }

        public ReviewSpoilerTagRemoved(Guid aggregateGuid, Guid readerGuid, int spoilerTagsCount)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            SpoilerTagsCount = spoilerTagsCount;
        }
    }
}