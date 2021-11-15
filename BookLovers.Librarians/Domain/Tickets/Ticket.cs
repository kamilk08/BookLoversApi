using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets.BusinessRules;
using BookLovers.Librarians.Events.Tickets;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class Ticket : AggregateRoot
    {
        public TicketContent TicketContent { get; private set; }

        public TicketDetails TicketDetails { get; private set; }

        public TicketState TicketState { get; private set; }

        public Decision Decision { get; private set; }

        public SolvedBy SolvedBy { get; private set; }

        public IssuedBy IssuedBy { get; private set; }

        private Ticket()
        {
        }

        internal Ticket(
            Guid ticketGuid,
            IssuedBy issuedBy,
            TicketContent ticketContent,
            TicketDetails ticketDetails)
        {
            this.Guid = ticketGuid;
            this.TicketContent = ticketContent;
            this.TicketDetails = ticketDetails;
            this.TicketState = TicketState.InProgress;
            this.IssuedBy = issuedBy;
            this.Decision = Decision.Unknown;
            this.SolvedBy = SolvedBy.NotSolvedByAnyOne();
            this.Status = AggregateStatus.Active.Value;
        }

        public void SolveTicket(Librarian librarian, Decision decision, string notification = null)
        {
            this.CheckBusinessRules(new SolveTicketRules(this));

            this.TicketState = TicketState.Solved;
            this.SolvedBy = new SolvedBy(librarian.Guid);
            this.Decision = decision;

            this.Events.Add(new TicketSolved(this.Guid, librarian.Guid, notification));
        }

        public bool IsSolved()
        {
            return this.TicketState == TicketState.Solved &&
                   this.SolvedBy.HasBeenSolved();
        }

        public bool IsSolvedBy(Librarian librarian)
        {
            var librarianGuid = this.SolvedBy.LibrarianGuid;

            if (!librarianGuid.HasValue)
                return false;

            if (librarianGuid.GetValueOrDefault() == Guid.Empty)
                return false;

            return this.SolvedBy.LibrarianGuid == librarianGuid;
        }

        public bool IsIssuedBy(TicketOwner ticketOwner)
        {
            return this.IssuedBy.TicketOwnerGuid == ticketOwner.Guid;
        }

        public bool IsTicketAbout(TicketConcern ticketConcern)
        {
            return this.TicketContent.TicketConcern == ticketConcern;
        }
    }
}