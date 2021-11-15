using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Publishers
{
    internal class AddPublisherCycleInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        private AddPublisherCycleInternalCommand()
        {
        }

        public AddPublisherCycleInternalCommand(Guid publisherGuid, Guid publisherCycleGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
            this.PublisherCycleGuid = publisherCycleGuid;
        }
    }
}