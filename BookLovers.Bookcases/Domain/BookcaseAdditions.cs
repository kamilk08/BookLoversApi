using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Bookcases.Domain
{
    public class BookcaseAdditions : ValueObject<BookcaseAdditions>
    {
        public Guid ReaderGuid { get; private set; }

        public Guid SettingsManagerGuid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        private BookcaseAdditions()
        {
        }

        public BookcaseAdditions(
            Guid readerGuid,
            Guid settingsManagerGuid,
            Guid shelfRecordTrackerGuid)
        {
            ReaderGuid = readerGuid;
            SettingsManagerGuid = settingsManagerGuid;
            ShelfRecordTrackerGuid = shelfRecordTrackerGuid;
        }

        public static BookcaseAdditions Create(Guid readerGuid) =>
            new BookcaseAdditions(readerGuid, Guid.NewGuid(), Guid.NewGuid());

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ReaderGuid.GetHashCode();
            hash = (hash * 23) + this.ShelfRecordTrackerGuid.GetHashCode();
            hash = (hash * 23) + this.SettingsManagerGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookcaseAdditions obj)
        {
            return ReaderGuid == obj.ReaderGuid
                   && SettingsManagerGuid == obj.SettingsManagerGuid
                   && ShelfRecordTrackerGuid == obj.ShelfRecordTrackerGuid;
        }
    }
}