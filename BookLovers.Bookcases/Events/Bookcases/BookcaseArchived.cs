using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Bookcases
{
    public class BookcaseArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int BookcaseStatus { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid SettingsManagerGuid { get; private set; }

        public Guid ShelfTrackerGuid { get; private set; }

        private BookcaseArchived()
        {
        }

        private BookcaseArchived(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid settingsManagerGuid,
            Guid shelfTrackerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            SettingsManagerGuid = settingsManagerGuid;
            ShelfTrackerGuid = shelfTrackerGuid;
            BookcaseStatus = AggregateStatus.Archived.Value;
        }

        public static BookcaseArchived Initialize()
        {
            return new BookcaseArchived();
        }

        public BookcaseArchived WithBookcase(Guid aggregateGuid)
        {
            return new BookcaseArchived(aggregateGuid, ReaderGuid, SettingsManagerGuid, ShelfTrackerGuid);
        }

        public BookcaseArchived WithReader(Guid readerGuid)
        {
            return new BookcaseArchived(AggregateGuid, readerGuid, SettingsManagerGuid, ShelfTrackerGuid);
        }

        public BookcaseArchived WithSettingsManager(Guid settingsMangerGuid)
        {
            return new BookcaseArchived(AggregateGuid, ReaderGuid, settingsMangerGuid, ShelfTrackerGuid);
        }

        public BookcaseArchived WithShelfTracker(Guid shelfTrackerGuid)
        {
            return new BookcaseArchived(AggregateGuid, ReaderGuid, SettingsManagerGuid, shelfTrackerGuid);
        }
    }
}