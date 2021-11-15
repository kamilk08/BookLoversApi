using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.PublisherCycles
{
    internal class AddPublisherCycleBookInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid CycleGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public bool SendIntegrationEvent { get; private set; }

        private AddPublisherCycleBookInternalCommand()
        {
        }

        public AddPublisherCycleBookInternalCommand(
            Guid cycleGuid,
            Guid bookGuid,
            bool sendIntegrationEvent)
        {
            this.Guid = Guid.NewGuid();
            this.CycleGuid = cycleGuid;
            this.BookGuid = bookGuid;
            this.SendIntegrationEvent = sendIntegrationEvent;
        }
    }
}