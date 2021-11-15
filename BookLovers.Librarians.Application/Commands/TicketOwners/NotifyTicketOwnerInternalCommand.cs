using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.TicketOwners
{
    internal class NotifyTicketOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid TicketGuid { get; private set; }

        public string Notification { get; private set; }

        private NotifyTicketOwnerInternalCommand()
        {
        }

        public NotifyTicketOwnerInternalCommand(Guid ticketGuid, string notification)
        {
            this.Guid = Guid.NewGuid();
            this.TicketGuid = ticketGuid;
            this.Notification = notification;
        }
    }
}