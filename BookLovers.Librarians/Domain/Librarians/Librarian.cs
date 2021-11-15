using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Librarians.Domain.Librarians.BusinessRules;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Events.Librarians;

namespace BookLovers.Librarians.Domain.Librarians
{
    public class Librarian : AggregateRoot
    {
        public Guid ReaderGuid { get; private set; }

        public IReadOnlyList<ResolvedTicket> Tickets => this._tickets.ToList<ResolvedTicket>();

        protected ICollection<ResolvedTicket> _tickets { get; set; } =
            new List<ResolvedTicket>();

        protected Librarian()
        {
        }

        public Librarian(Guid librarianGuid, Guid readerGuid)
        {
            this.Guid = librarianGuid;
            this.ReaderGuid = readerGuid;
            this.Status = AggregateStatus.Active.Value;
            this.Events.Add(new LibrarianCreated(librarianGuid, readerGuid));
        }

        public void ResolveTicket(
            Ticket ticket,
            Decision decision,
            DecisionJustification justification,
            IDecisionChecker decisionChecker)
        {
            this.CheckBusinessRules(new ResolveTicketRules(this, ticket, decisionChecker));

            this._tickets.Add(new ResolvedTicket(ticket.Guid, decision, justification));

            var @event = LibrarianResolvedTicket
                .Initialize()
                .WithAggregate(this.Guid)
                .WithTicket(ticket.Guid, ticket.TicketContent.TicketConcern.Value)
                .WithDecision(decision.Value, justification.Content);

            this.Events.Add(@event);
        }

        public ResolvedTicket GetTicket(Guid ticketGuid)
        {
            return this._tickets.SingleOrDefault(p => p.TicketGuid == ticketGuid);
        }

        public bool HasResolvedTicket(Guid ticketGuid)
        {
            return this.GetTicket(ticketGuid) != null;
        }

        public IEnumerable<ResolvedTicket> GetAllApprovedTickets()
        {
            return this._tickets.Where(p => p.Decision == Decision.Approve);
        }

        public class Relations
        {
            public const string ResolvedTicketsCollectionName = "_tickets";

            public static Expression<Func<Librarian, ICollection<ResolvedTicket>>> ResolvedTickets =>
                librarian => librarian._tickets;
        }
    }
}