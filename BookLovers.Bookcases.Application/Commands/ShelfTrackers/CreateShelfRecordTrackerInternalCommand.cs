using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.ShelfTrackers
{
    internal class CreateShelfRecordTrackerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        private CreateShelfRecordTrackerInternalCommand()
        {
        }

        public CreateShelfRecordTrackerInternalCommand(Guid bookcaseGuid, Guid shelfRecordTrackerGuid)
        {
            Guid = Guid.NewGuid();
            BookcaseGuid = bookcaseGuid;
            ShelfRecordTrackerGuid = shelfRecordTrackerGuid;
        }
    }
}