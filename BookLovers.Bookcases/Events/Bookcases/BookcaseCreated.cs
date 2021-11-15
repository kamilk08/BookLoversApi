using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Bookcases
{
    public class BookcaseCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid SettingsManagerGuid { get; private set; }

        public Guid ShelfTrackerGuid { get; private set; }

        public int BookcaseStatus { get; private set; }

        public int ReaderId { get; private set; }

        private BookcaseCreated()
        {
        }

        private BookcaseCreated(
            Guid bookcaseGuid,
            Guid readerGuid,
            int readerId,
            Guid settingsManagerGuid,
            Guid shelfTrackerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = bookcaseGuid;
            ReaderGuid = readerGuid;
            ReaderId = readerId;
            SettingsManagerGuid = settingsManagerGuid;
            ShelfTrackerGuid = shelfTrackerGuid;
            BookcaseStatus = AggregateStatus.Active.Value;
        }

        public static BookcaseCreated Initialize()
        {
            return new BookcaseCreated();
        }

        public BookcaseCreated WithBookcase(Guid bookcaseGuid)
        {
            return new BookcaseCreated(bookcaseGuid, ReaderGuid, ReaderId, SettingsManagerGuid, ShelfTrackerGuid);
        }

        public BookcaseCreated WithReader(Guid readerGuid, int readerId)
        {
            return new BookcaseCreated(AggregateGuid, readerGuid, readerId, SettingsManagerGuid, ShelfTrackerGuid);
        }

        public BookcaseCreated WithSettingsManager(Guid settingsManagerGuid)
        {
            return new BookcaseCreated(AggregateGuid, ReaderGuid, ReaderId, settingsManagerGuid, ShelfTrackerGuid);
        }

        public BookcaseCreated WithShelfRecordTracker(Guid shelfRecordTrackerGuid)
        {
            return new BookcaseCreated(AggregateGuid, ReaderGuid, ReaderId, SettingsManagerGuid,
                shelfRecordTrackerGuid);
        }
    }
}