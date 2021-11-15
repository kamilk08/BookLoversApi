using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.ShelfTrackers
{
    internal class ArchiveShelfRecordTrackerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        private ArchiveShelfRecordTrackerInternalCommand()
        {
        }

        public ArchiveShelfRecordTrackerInternalCommand(Guid shelfRecordTrackerGuid)
        {
            Guid = Guid.NewGuid();
            ShelfRecordTrackerGuid = shelfRecordTrackerGuid;
        }
    }
}