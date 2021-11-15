using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.ShelfTrackers
{
    internal class TrackBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime TrackedAt { get; private set; }

        private TrackBookInternalCommand()
        {
        }

        public TrackBookInternalCommand(
            Guid shelfRecordTrackerGuid,
            Guid shelfGuid,
            Guid bookGuid,
            DateTime trackedAt)
        {
            Guid = Guid.NewGuid();
            ShelfRecordTrackerGuid = shelfRecordTrackerGuid;
            ShelfGuid = shelfGuid;
            BookGuid = bookGuid;
            TrackedAt = trackedAt;
        }
    }
}