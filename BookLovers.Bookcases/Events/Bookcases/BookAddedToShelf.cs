using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Bookcases
{
    public class BookAddedToShelf : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid TrackerGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        private BookAddedToShelf()
        {
        }

        private BookAddedToShelf(
            Guid aggregateGuid,
            Guid bookGuid,
            Guid shelfGuid,
            Guid readerGuid,
            Guid trackerGuid,
            DateTime addedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookGuid = bookGuid;
            ShelfGuid = shelfGuid;
            ReaderGuid = readerGuid;
            TrackerGuid = trackerGuid;
            AddedAt = addedAt;
        }

        public static BookAddedToShelf Initialize()
        {
            return new BookAddedToShelf();
        }

        public BookAddedToShelf WithBookcase(Guid aggregateGuid)
        {
            return new BookAddedToShelf(aggregateGuid, BookGuid, ShelfGuid, ReaderGuid, TrackerGuid, AddedAt);
        }

        public BookAddedToShelf WithBookAndShelf(Guid bookGuid, Guid shelfGuid)
        {
            return new BookAddedToShelf(AggregateGuid, bookGuid, shelfGuid, ReaderGuid, TrackerGuid, AddedAt);
        }

        public BookAddedToShelf WithReader(Guid readerGuid)
        {
            return new BookAddedToShelf(
                AggregateGuid,
                BookGuid,
                ShelfGuid,
                readerGuid,
                TrackerGuid,
                AddedAt);
        }

        public BookAddedToShelf WithTracker(Guid trackerGuid)
        {
            return new BookAddedToShelf(
                AggregateGuid,
                BookGuid,
                ShelfGuid,
                ReaderGuid,
                trackerGuid,
                AddedAt);
        }

        public BookAddedToShelf WithAddedAt(DateTime addedAt)
        {
            return new BookAddedToShelf(
                AggregateGuid,
                BookGuid,
                ShelfGuid,
                ReaderGuid,
                TrackerGuid,
                addedAt);
        }
    }
}