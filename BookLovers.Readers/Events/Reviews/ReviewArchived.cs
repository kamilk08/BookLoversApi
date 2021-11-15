using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int ReviewStatus { get; private set; }

        private ReviewArchived()
        {
        }

        [JsonConstructor]
        protected ReviewArchived(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            Guid bookGuid,
            int reviewStatus)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            ReviewStatus = reviewStatus;
        }

        public ReviewArchived(Guid reviewGuid, Guid readerGuid, Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = reviewGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            ReviewStatus = AggregateStatus.Archived.Value;
        }
    }
}