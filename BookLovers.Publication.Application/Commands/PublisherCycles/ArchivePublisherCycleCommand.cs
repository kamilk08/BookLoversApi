using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.PublisherCycles
{
    public class ArchivePublisherCycleCommand : ICommand
    {
        public Guid PublisherCycleGuid { get; }

        public ArchivePublisherCycleCommand(Guid publisherCycleGuid)
        {
            this.PublisherCycleGuid = publisherCycleGuid;
        }
    }
}