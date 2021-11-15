using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules;
using BookLovers.Bookcases.Events.ShelfRecordTracker;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker
{
    public class ShelfRecordTracker :
        EventSourcedAggregateRoot,
        IHandle<BookTracked>,
        IHandle<BookReTracked>,
        IHandle<BookUnTracked>,
        IHandle<ShelfRecordTrackerCreated>,
        IHandle<ShelfRecordTrackerArchived>
    {
        private readonly List<ShelfRecord> _shelfRecords = new List<ShelfRecord>();

        public Guid BookcaseGuid { get; private set; }

        public IReadOnlyList<ShelfRecord> ShelfRecords => _shelfRecords.ToList();

        private ShelfRecordTracker()
        {
        }

        public ShelfRecordTracker(Guid trackerGuid, Guid bookcaseGuid)
        {
            Guid = trackerGuid;
            BookcaseGuid = bookcaseGuid;
            AggregateStatus = AggregateStatus.Active;

            ApplyChange(new ShelfRecordTrackerCreated(Guid, bookcaseGuid));
        }

        public void TrackBook(ShelfRecord shelfRecord)
        {
            CheckBusinessRules(new TrackBookRules(this, shelfRecord));

            var @event = new BookTracked(Guid)
                .WithBookcase(BookcaseGuid)
                .WithBook(shelfRecord.BookGuid)
                .WithShelf(shelfRecord.ShelfGuid)
                .WithTrackedAt(shelfRecord.TrackedAt);

            ApplyChange(@event);
        }

        public void ReTrackBook(ShelfRecord oldShelfRecord, ShelfRecord newShelfRecord)
        {
            CheckBusinessRules(new ReTrackBookRules(this, oldShelfRecord, newShelfRecord));

            var @event = new BookReTracked(Guid)
                .WithBook(oldShelfRecord.BookGuid)
                .WithOldShelf(oldShelfRecord.ShelfGuid)
                .WithNewShelf(newShelfRecord.ShelfGuid)
                .WithTrackedAt(newShelfRecord.TrackedAt);

            ApplyChange(@event);
        }

        public void UnTrackBook(ShelfRecord shelfRecord)
        {
            CheckBusinessRules(new UnTrackBookRules(this, shelfRecord));

            var @event = new BookUnTracked(Guid)
                .WithBookcase(BookcaseGuid)
                .WithBook(shelfRecord.BookGuid)
                .WithShelf(shelfRecord.ShelfGuid)
                .WithTrackedAt(shelfRecord.TrackedAt);

            ApplyChange(@event);
        }

        public ShelfRecord GetOldTrackedRecord(Guid bookGuid, Guid shelfGuid, DateTime date)
        {
            return ShelfRecords.SingleOrDefault(p =>
                p.BookGuid == bookGuid && p.ShelfGuid == shelfGuid && !p.IsTracked &&
                p.TrackedAt.AreEqualWithoutTicks(date));
        }

        public ShelfRecord GetTrackedBook(Guid bookGuid, Guid shelfGuid)
        {
            return _shelfRecords.SingleOrDefault(p =>
                p.BookGuid == bookGuid && p.ShelfGuid == shelfGuid && p.IsTracked);
        }

        void IHandle<BookTracked>.Handle(BookTracked @event)
        {
            _shelfRecords.Add(new ShelfRecord(@event.ShelfGuid, @event.BookGuid, @event.AddedAt));
        }

        void IHandle<BookUnTracked>.Handle(BookUnTracked @event)
        {
            var trackedBook = GetTrackedBook(@event.BookGuid, @event.ShelfGuid);

            _shelfRecords.Remove(trackedBook);

            _shelfRecords.Add(trackedBook.UnTrack());
        }

        void IHandle<BookReTracked>.Handle(BookReTracked @event)
        {
            var trackedBook = GetTrackedBook(@event.BookGuid, @event.OldShelfGuid);

            _shelfRecords.Remove(trackedBook);

            _shelfRecords.Add(trackedBook.UnTrack());

            _shelfRecords.Add(new ShelfRecord(@event.NewShelfGuid, @event.BookGuid, @event.TrackedAt));
        }

        void IHandle<ShelfRecordTrackerCreated>.Handle(
            ShelfRecordTrackerCreated @event)
        {
            Guid = @event.AggregateGuid;
            BookcaseGuid = @event.BookcaseGuid;
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<ShelfRecordTrackerArchived>.Handle(
            ShelfRecordTrackerArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }
    }
}