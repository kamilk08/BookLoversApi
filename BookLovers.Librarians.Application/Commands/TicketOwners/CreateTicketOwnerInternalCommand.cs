using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.TicketOwners
{
    internal class CreateTicketOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid TicketOwnerGuid { get; private set; }

        public int TicketOwnerId { get; private set; }

        private CreateTicketOwnerInternalCommand()
        {
        }

        public CreateTicketOwnerInternalCommand(Guid ticketOwnerGuid, int ticketOwnerId)
        {
            this.Guid = Guid.NewGuid();
            this.TicketOwnerGuid = ticketOwnerGuid;
            this.TicketOwnerId = ticketOwnerId;
        }
    }
}