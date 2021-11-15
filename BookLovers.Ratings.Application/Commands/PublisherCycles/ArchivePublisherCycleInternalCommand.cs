using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.PublisherCycles
{
    internal class ArchivePublisherCycleInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        public ArchivePublisherCycleInternalCommand(Guid publisherCycleGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherCycleGuid = publisherCycleGuid;
        }
    }
}