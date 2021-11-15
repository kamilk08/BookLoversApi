using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Publishers
{
    internal class AddPublisherBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public bool SendIntegrationEvent { get; private set; }

        private AddPublisherBookInternalCommand()
        {
        }

        public AddPublisherBookInternalCommand(
            Guid publisherGuid,
            Guid bookGuid,
            bool sendIntegrationEvent)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherGuid = publisherGuid;
            this.BookGuid = bookGuid;
            this.SendIntegrationEvent = sendIntegrationEvent;
        }
    }
}