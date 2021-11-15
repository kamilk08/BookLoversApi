using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.Tickets
{
    internal class ArchiveTicketInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid TicketGuid { get; private set; }

        private ArchiveTicketInternalCommand()
        {
        }

        public ArchiveTicketInternalCommand(Guid ticketGuid)
        {
            this.Guid = Guid.NewGuid();
            this.TicketGuid = ticketGuid;
        }
    }
}