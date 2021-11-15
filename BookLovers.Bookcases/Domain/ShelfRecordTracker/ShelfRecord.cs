using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker
{
    public class ShelfRecord : ValueObject<ShelfRecord>
    {
        public Guid ShelfGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime TrackedAt { get; private set; }

        public bool IsTracked { get; private set; }

        private ShelfRecord()
        {
        }

        private ShelfRecord(Guid shelfGuid, Guid bookGuid, DateTime trackedAt, bool isTracked)
        {
            ShelfGuid = shelfGuid;
            BookGuid = bookGuid;
            TrackedAt = trackedAt;
            IsTracked = isTracked;
        }

        public ShelfRecord(Guid shelfGuid, Guid bookGuid, DateTime trackedAt)
        {
            ShelfGuid = shelfGuid;
            BookGuid = bookGuid;
            TrackedAt = trackedAt;
            IsTracked = true;
        }

        public ShelfRecord UnTrack() => new ShelfRecord(ShelfGuid, BookGuid, TrackedAt, false);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + BookGuid.GetHashCode();
            hash = (hash * 23) + ShelfGuid.GetHashCode();
            hash = (hash * 23) + TrackedAt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ShelfRecord obj) =>
            ShelfGuid == obj.ShelfGuid
            && BookGuid == obj.BookGuid
            && TrackedAt == obj.TrackedAt;
    }
}