using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.TicketOwners
{
    internal class SuspendTicketOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid TicketOwnerGuid { get; private set; }

        public Guid Guid { get; private set; }

        private SuspendTicketOwnerInternalCommand()
        {
        }

        public SuspendTicketOwnerInternalCommand(Guid ticketOwnerGuid)
        {
            this.TicketOwnerGuid = ticketOwnerGuid;
            this.Guid = Guid.NewGuid();
        }
    }
}