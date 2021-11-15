using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.Tickets
{
    internal class SolveTicketInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid TicketGuid { get; private set; }

        public Guid LibrarianGuid { get; private set; }

        public int DecisionId { get; private set; }

        public string Notification { get; private set; }

        private SolveTicketInternalCommand()
        {
        }

        public SolveTicketInternalCommand(
            Guid ticketGuid,
            Guid librarianGuid,
            int decisionId,
            string notification)
        {
            this.Guid = Guid.NewGuid();
            this.TicketGuid = ticketGuid;
            this.LibrarianGuid = librarianGuid;
            this.DecisionId = decisionId;
            this.Notification = notification;
        }
    }
}