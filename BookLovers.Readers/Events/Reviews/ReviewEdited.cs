using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewEdited : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public string Review { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime EditedAt { get; private set; }

        public bool ContainsSpoilers { get; private set; }

        private ReviewEdited()
        {
        }

        [JsonConstructor]
        protected ReviewEdited(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            string review,
            DateTime createdAt,
            DateTime editedAt,
            bool containsSpoilers)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            Review = review;
            CreatedAt = createdAt;
            EditedAt = editedAt;
            ContainsSpoilers = containsSpoilers;
        }

        private ReviewEdited(
            Guid aggregateGuid,
            Guid readerGuid,
            string review,
            DateTime createdAt,
            DateTime editedAt,
            bool containsSpoilers)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            Review = review;
            CreatedAt = createdAt;
            EditedAt = editedAt;
            ContainsSpoilers = containsSpoilers;
        }

        public static ReviewEdited Initialize()
        {
            return new ReviewEdited();
        }

        public ReviewEdited WithAggregate(Guid aggregateGuid)
        {
            return new ReviewEdited(aggregateGuid, ReaderGuid, Review,
                CreatedAt, EditedAt, ContainsSpoilers);
        }

        public ReviewEdited WithReader(Guid readerGuid)
        {
            return new ReviewEdited(AggregateGuid, readerGuid, Review,
                CreatedAt, EditedAt, ContainsSpoilers);
        }

        public ReviewEdited WithContent(string review, bool containsSpoilers)
        {
            return new ReviewEdited(
                AggregateGuid,
                ReaderGuid, review, CreatedAt, EditedAt, containsSpoilers);
        }

        public ReviewEdited WithDates(DateTime createdAt, DateTime editedAt)
        {
            return new ReviewEdited(
                AggregateGuid,
                ReaderGuid, Review, createdAt, editedAt, ContainsSpoilers);
        }
    }
}