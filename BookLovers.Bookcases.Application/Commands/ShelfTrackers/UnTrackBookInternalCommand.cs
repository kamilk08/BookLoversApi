using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.ShelfTrackers
{
    internal class UnTrackBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private UnTrackBookInternalCommand()
        {
        }

        public UnTrackBookInternalCommand(Guid shelfRecordTrackerGuid, Guid shelfGuid, Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            ShelfRecordTrackerGuid = shelfRecordTrackerGuid;
            ShelfGuid = shelfGuid;
            BookGuid = bookGuid;
        }
    }
}