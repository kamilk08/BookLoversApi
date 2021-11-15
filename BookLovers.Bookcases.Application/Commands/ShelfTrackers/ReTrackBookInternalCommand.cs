using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.ShelfTrackers
{
    internal class ReTrackBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        public Guid OldShelfGuid { get; private set; }

        public Guid NewShelfGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime ChangedAt { get; private set; }

        public ReTrackBookInternalCommand(
            Guid trackerGuid,
            Guid oldShelfGuid,
            Guid newShelfGuid,
            Guid bookGuid,
            DateTime changedAt)
        {
            Guid = Guid.NewGuid();
            ShelfRecordTrackerGuid = trackerGuid;
            OldShelfGuid = oldShelfGuid;
            NewShelfGuid = newShelfGuid;
            BookGuid = bookGuid;
            ChangedAt = changedAt;
        }
    }
}