using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class AddAuthorBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public bool SendIntegrationEvent { get; private set; }

        private AddAuthorBookInternalCommand()
        {
        }

        public AddAuthorBookInternalCommand(Guid authorGuid, Guid bookGuid, bool sendIntegrationEvent)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.BookGuid = bookGuid;
            this.SendIntegrationEvent = sendIntegrationEvent;
        }
    }
}