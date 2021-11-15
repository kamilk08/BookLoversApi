using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Publishers
{
    internal class RemovePublisherCycleInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        private RemovePublisherCycleInternalCommand()
        {
        }

        public RemovePublisherCycleInternalCommand(Guid publisherGuid, Guid publisherCycleGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
            this.PublisherCycleGuid = publisherCycleGuid;
        }
    }
}