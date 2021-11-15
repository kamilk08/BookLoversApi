using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Domain.Reviews;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewCreated : IEvent
    {
        public int ReviewStatus { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public bool MarkedAsSpoiler { get; private set; }

        public bool MarkedAsSpoilerByOthers { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime EditDate { get; private set; }

        public string Review { get; private set; }

        public int Likes { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private ReviewCreated()
        {
        }

        [JsonConstructor]
        protected ReviewCreated(
            int reviewStatus,
            Guid guid,
            Guid aggregateGuid,
            bool markedAsSpoiler,
            bool markedAsSpoilerByOthers,
            DateTime createdAt,
            DateTime editDate,
            string review,
            int likes,
            Guid bookGuid,
            Guid readerGuid)
        {
            ReviewStatus = reviewStatus;
            Guid = guid;
            AggregateGuid = aggregateGuid;
            MarkedAsSpoiler = markedAsSpoiler;
            MarkedAsSpoilerByOthers = markedAsSpoilerByOthers;
            CreatedAt = createdAt;
            EditDate = editDate;
            Review = review;
            Likes = likes;
            BookGuid = bookGuid;
            ReaderGuid = readerGuid;
        }

        private ReviewCreated(
            Guid reviewGuid,
            Guid readerGuid,
            Guid bookGuid,
            string review,
            DateTime createdAt,
            DateTime editDate,
            bool markedAsSpoiler,
            bool markedAsSpoilerByOthers)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = reviewGuid;
            Likes = ReviewExtensions.NoLikes;
            Review = review;
            CreatedAt = createdAt;
            EditDate = editDate;
            ReviewStatus = AggregateStatus.Active.Value;
            BookGuid = bookGuid;
            ReaderGuid = readerGuid;
            MarkedAsSpoiler = markedAsSpoiler;
            MarkedAsSpoilerByOthers = markedAsSpoilerByOthers;
        }

        public static ReviewCreated Initialize()
        {
            return new ReviewCreated();
        }

        public ReviewCreated WithAggregate(Guid aggregateGuid)
        {
            return new ReviewCreated(aggregateGuid, ReaderGuid, BookGuid,
                Review, CreatedAt, EditDate,
                MarkedAsSpoiler, MarkedAsSpoilerByOthers);
        }

        public ReviewCreated WithReader(Guid readerGuid)
        {
            return new ReviewCreated(AggregateGuid, readerGuid, BookGuid,
                Review, CreatedAt, EditDate,
                MarkedAsSpoiler, MarkedAsSpoilerByOthers);
        }

        public ReviewCreated WithBook(Guid bookGuid)
        {
            return new ReviewCreated(AggregateGuid, ReaderGuid, bookGuid, Review,
                CreatedAt, EditDate, MarkedAsSpoiler, MarkedAsSpoilerByOthers);
        }

        public ReviewCreated WithContent(string review)
        {
            return new ReviewCreated(AggregateGuid, ReaderGuid, BookGuid,
                review, CreatedAt, EditDate, MarkedAsSpoiler, MarkedAsSpoilerByOthers);
        }

        public ReviewCreated WithDates(DateTime createdAt, DateTime editDate)
        {
            return new ReviewCreated(
                AggregateGuid,
                ReaderGuid, BookGuid, Review, createdAt,
                editDate, MarkedAsSpoiler, MarkedAsSpoilerByOthers);
        }

        public ReviewCreated WithSpoiler(
            bool markedAsSpoiler,
            bool markedAsSpoilerByOthers)
        {
            return new ReviewCreated(AggregateGuid, ReaderGuid, BookGuid, Review, CreatedAt, EditDate, markedAsSpoiler,
                markedAsSpoilerByOthers);
        }
    }
}