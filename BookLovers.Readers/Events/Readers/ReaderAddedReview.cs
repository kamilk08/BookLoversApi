using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderAddedReview : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        private ReaderAddedReview()
        {
        }

        [JsonConstructor]
        protected ReaderAddedReview(
            Guid guid,
            Guid aggregateGuid,
            Guid reviewGuid,
            Guid bookGuid,
            DateTime addedAt)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReviewGuid = reviewGuid;
            BookGuid = bookGuid;
            AddedAt = addedAt;
        }

        private ReaderAddedReview(
            Guid aggregateGuid,
            Guid reviewGuid,
            Guid bookGuid,
            DateTime addedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReviewGuid = reviewGuid;
            BookGuid = bookGuid;
            AddedAt = addedAt;
        }

        public static ReaderAddedReview Initialize()
        {
            return new ReaderAddedReview();
        }

        public ReaderAddedReview WithAggregate(Guid aggregateGuid)
        {
            return new ReaderAddedReview(aggregateGuid, ReviewGuid, BookGuid, AddedAt);
        }

        public ReaderAddedReview WithBookReview(Guid reviewGuid, Guid bookGuid)
        {
            return new ReaderAddedReview(AggregateGuid, reviewGuid, bookGuid, AddedAt);
        }

        public ReaderAddedReview WithAddedAt(DateTime addedAt)
        {
            return new ReaderAddedReview(AggregateGuid, ReviewGuid, BookGuid, addedAt);
        }
    }
}