using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.TicketOwners
{
    internal class AddTicketToTheOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid TicketGuid { get; private set; }

        public Guid TicketOwnerGuid { get; private set; }

        private AddTicketToTheOwnerInternalCommand()
        {
        }

        public AddTicketToTheOwnerInternalCommand(Guid ticketGuid, Guid ticketOwnerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.TicketGuid = ticketGuid;
            this.TicketOwnerGuid = ticketOwnerGuid;
        }
    }
}