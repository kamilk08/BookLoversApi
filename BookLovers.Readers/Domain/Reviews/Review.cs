using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Reviews.BusinessRules;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Mementos;
using BookLovers.Shared.Likes;

namespace BookLovers.Readers.Domain.Reviews
{
    [AllowSnapshot]
    public class Review :
        EventSourcedAggregateRoot,
        IHandle<ReviewCreated>,
        IHandle<ReviewEdited>,
        IHandle<ReviewLiked>,
        IHandle<ReviewUnLiked>,
        IHandle<ReviewReported>,
        IHandle<ReviewArchived>,
        IHandle<ReviewSpoilerTagRemoved>,
        IHandle<ReviewMarkToggledByReader>,
        IHandle<ReviewMarkedByOtherReaders>,
        IHandle<ReviewUnMarkedByOtherReaders>,
        IHandle<NewReviewSpoilerTag>
    {
        private List<Like> _likes = new List<Like>();

        private List<ReviewReport> _reports = new List<ReviewReport>();

        private List<SpoilerTag> _spoilerTags = new List<SpoilerTag>();

        public IReadOnlyList<Like> Likes => _likes;

        public IReadOnlyList<ReviewReport> Reports => _reports;

        public IReadOnlyList<SpoilerTag> SpoilerTags => _spoilerTags;

        public ReviewContent ReviewContent { get; private set; }

        public ReviewIdentification ReviewIdentification { get; private set; }

        public ReviewSpoiler ReviewSpoiler { get; private set; }

        protected Review()
        {
        }

        public Review(
            Guid reviewGuid,
            ReviewIdentification identification,
            ReviewContent reviewContent,
            ReviewSpoiler reviewSpoiler)
        {
            Guid = reviewGuid;
            ReviewIdentification = identification;
            ReviewContent = reviewContent;
            ReviewSpoiler = reviewSpoiler;
            AggregateStatus = AggregateStatus.Active;

            var @event = ReviewCreated.Initialize()
                .WithAggregate(reviewGuid)
                .WithReader(ReviewIdentification.ReaderGuid)
                .WithBook(ReviewIdentification.BookGuid)
                .WithContent(reviewContent.Review)
                .WithDates(reviewContent.ReviewDate, reviewContent.EditedDate.GetValueOrDefault())
                .WithSpoiler(reviewSpoiler.MarkedAsSpoilerByReader, false);

            ApplyChange(@event);
        }

        public void EditReview(Guid readerGuid, ReviewContent reviewContent, bool containsSpoilers)
        {
            CheckBusinessRules(new EditReviewRules(this, readerGuid));

            var @event = ReviewEdited.Initialize()
                .WithAggregate(Guid)
                .WithReader(ReviewIdentification.ReaderGuid)
                .WithContent(reviewContent.Review, containsSpoilers)
                .WithDates(
                    reviewContent.ReviewDate,
                    reviewContent.EditedDate.GetValueOrDefault());

            ApplyChange(@event);
        }

        public void AddLike(Like like)
        {
            CheckBusinessRules(new AddReviewLikeRules(this, like));

            var @event = new ReviewLiked(Guid, like.ReaderGuid, ReviewIdentification.ReaderGuid,
                _likes.Count + ReviewExtensions.OneLike);

            ApplyChange(@event);
        }

        public void RemoveLike(Like like)
        {
            CheckBusinessRules(new RemoveLikeReviewRules(this, like));

            var @event = new ReviewUnLiked(Guid, like.ReaderGuid, ReviewIdentification.ReaderGuid,
                _likes.Count - ReviewExtensions.OneLike);

            ApplyChange(@event);
        }

        public void Report(ReviewReport reviewReport)
        {
            CheckBusinessRules(new ReportReviewRules(this, reviewReport));

            ApplyChange(new ReviewReported(Guid, reviewReport.ReportedBy, ReviewIdentification.ReaderGuid));
        }

        public void AddOrRemoveSpoilerMark()
        {
            CheckBusinessRules(new AggregateMustBeActive(AggregateStatus.Value));

            ApplyChange(new ReviewMarkToggledByReader(Guid, !ReviewSpoiler.MarkedAsSpoilerByReader));
        }

        public void AddSpoilerTag(SpoilerTag spoilerTag)
        {
            CheckBusinessRules(new AddSpoilerTagRules(this, spoilerTag));

            ApplyChange(new NewReviewSpoilerTag(Guid, spoilerTag.ReaderGuid));

            if (!SuitableForMark())
                return;

            ApplyChange(new ReviewMarkedByOtherReaders(Guid));
        }

        public void RemoveSpoilerTag(SpoilerTag spoilerTag)
        {
            CheckBusinessRules(new RemoveSpoilerTagRules(this, spoilerTag));

            ApplyChange(new ReviewSpoilerTagRemoved(Guid, spoilerTag.ReaderGuid,
                _spoilerTags.Count - ReviewExtensions.SingleSpoilerTag));

            if (!IsSuitableForUnMark())
                return;

            ApplyChange(new ReviewUnMarkedByOtherReaders(Guid));
        }

        public ReviewReport GetReviewReport(Guid readerGuid)
        {
            return _reports.Find(p => p.ReportedBy == readerGuid);
        }

        public SpoilerTag GetSpoilerTag(Guid readerGuid)
        {
            return _spoilerTags.Find(p => p.ReaderGuid == readerGuid);
        }

        public bool HasBeenReportedBy(Guid readerGuid)
        {
            return GetReviewReport(readerGuid) != null;
        }

        public Like GetLike(Guid readerGuid)
        {
            return _likes.Find(p => p.ReaderGuid == readerGuid);
        }

        public bool IsLikedBy(Guid readerGuid)
        {
            return GetLike(readerGuid) != null;
        }

        private bool SuitableForMark()
        {
            return !ReviewSpoiler.MarkedByOtherReaders &&
                   _spoilerTags.Count >= ReviewExtensions.SpoilerBorderCount;
        }

        private bool IsSuitableForUnMark()
        {
            return ReviewSpoiler.MarkedByOtherReaders &&
                   SpoilerTags.Count < ReviewExtensions.SpoilerBorderCount;
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var reviewMemento = memento as IReviewMemento;

            Guid = reviewMemento.AggregateGuid;
            AggregateStatus = AggregateStates.Get(reviewMemento.AggregateStatus);
            Version = reviewMemento.Version;
            LastCommittedVersion = reviewMemento.LastCommittedVersion;

            ReviewIdentification = new ReviewIdentification(reviewMemento.BookGuid, reviewMemento.ReaderGuid);
            ReviewContent = new ReviewContent(reviewMemento.Review, reviewMemento.EditedAt.GetValueOrDefault(),
                reviewMemento.CreatedAt);
            ReviewSpoiler =
                new ReviewSpoiler(reviewMemento.MarkedAsSpoilerByReader, reviewMemento.MarkedByOtherReaders);

            _likes = reviewMemento.Likes.Select(s => Like.NewLike(s)).ToList();
            _reports = reviewMemento.ReviewReports.Select(s => new ReviewReport(s)).ToList();
            _spoilerTags = reviewMemento.SpoilerTags.Select(s => new SpoilerTag(s)).ToList();
        }

        void IHandle<ReviewCreated>.Handle(ReviewCreated @event)
        {
            Guid = @event.AggregateGuid;
            ReviewIdentification = new ReviewIdentification(@event.BookGuid, @event.ReaderGuid);
            AggregateStatus = AggregateStatus.Active;
            ReviewContent = new ReviewContent(@event.Review, @event.CreatedAt);
            ReviewSpoiler = new ReviewSpoiler(@event.MarkedAsSpoiler, false);
        }

        void IHandle<ReviewEdited>.Handle(ReviewEdited @event)
        {
            Guid = @event.AggregateGuid;
            ReviewContent = ReviewContent.EditReview(@event.Review, @event.EditedAt, @event.CreatedAt);
        }

        void IHandle<ReviewLiked>.Handle(ReviewLiked @event)
        {
            _likes.Add(Like.NewLike(@event.LikeGiverGuid));
        }

        void IHandle<ReviewUnLiked>.Handle(ReviewUnLiked @event)
        {
            var like = _likes.Find(p => p.ReaderGuid == @event.LikeGiverGuid);

            _likes.Remove(like);
        }

        void IHandle<ReviewReported>.Handle(ReviewReported @event)
        {
            _reports.Add(new ReviewReport(@event.ReportedByGuid));
        }

        void IHandle<NewReviewSpoilerTag>.Handle(NewReviewSpoilerTag @event)
        {
            _spoilerTags.Add(new SpoilerTag(@event.ReaderGuid));
        }

        void IHandle<ReviewSpoilerTagRemoved>.Handle(
            ReviewSpoilerTagRemoved @event)
        {
            var spoilerTag = _spoilerTags.Find(p => p.ReaderGuid == @event.ReaderGuid);

            _spoilerTags.Remove(spoilerTag);
        }

        void IHandle<ReviewMarkToggledByReader>.Handle(
            ReviewMarkToggledByReader @event)
        {
            ReviewSpoiler = @event.MarkedAsASpoiler ? ReviewSpoiler.MarkedByReader() : ReviewSpoiler.UnMarkedByReader();
        }

        void IHandle<ReviewMarkedByOtherReaders>.Handle(
            ReviewMarkedByOtherReaders @event)
        {
            ReviewSpoiler = ReviewSpoiler.MarkedByOthers();
        }

        void IHandle<ReviewUnMarkedByOtherReaders>.Handle(
            ReviewUnMarkedByOtherReaders @event)
        {
            ReviewSpoiler = ReviewSpoiler.UnMarkedByOthers();
        }

        void IHandle<ReviewArchived>.Handle(ReviewArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }
    }
}