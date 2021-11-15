using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.PublisherCycles
{
    internal class CreatePublisherCycleInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        public int PublisherCycleId { get; private set; }

        private CreatePublisherCycleInternalCommand()
        {
        }

        public CreatePublisherCycleInternalCommand(Guid publisherCycleGuid, int publisherCycleId)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherCycleGuid = publisherCycleGuid;
            this.PublisherCycleId = publisherCycleId;
        }
    }
}