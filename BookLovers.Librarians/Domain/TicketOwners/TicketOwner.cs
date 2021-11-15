using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Librarians.Domain.TicketOwners.BusinessRules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.TicketOwners
{
    public class TicketOwner : AggregateRoot
    {
        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        protected ICollection<CreatedTicket> _tickets { get; set; } =
            new List<CreatedTicket>();

        public IReadOnlyList<CreatedTicket> Tickets => _tickets.ToList();

        private TicketOwner()
        {
        }

        public TicketOwner(Guid aggregateGuid, Guid readerGuid, int readerId)
        {
            Guid = aggregateGuid;
            ReaderGuid = readerGuid;
            ReaderId = readerId;
            Status = AggregateStatus.Active.Value;
        }

        public void AddTicket(Ticket ticket)
        {
            CheckBusinessRules(new AddTicketRules(this, ticket));

            _tickets.Add(new CreatedTicket(ticket.Guid, false));
        }

        public void NotifyOwner(IOwnerNotification ownerNotification)
        {
            CheckBusinessRules(
                new NotifyOwnerRules(
                    this,
                    ownerNotification.ReaderGuid,
                    ownerNotification.TicketGuid));

            SolvePendingTicket(ownerNotification.TicketGuid);

            Events.Add(ownerNotification);
        }

        public bool IsTicketSolved(Guid ticketGuid)
        {
            var createdTicket = _tickets.SingleOrDefault(p => p.TicketGuid == ticketGuid);

            return createdTicket != null && createdTicket.IsSolved;
        }

        public bool HasPendingTickets() => _tickets.Any(a => !a.IsSolved);

        public IEnumerable<CreatedTicket> GetAllPendingTickets() => _tickets
            .Where(a => !a.IsSolved).AsEnumerable();

        private void SolvePendingTicket(Guid ticketGuid) => _tickets
            .Single(p => p.TicketGuid == ticketGuid).MarkAsSolved();

        public static class Relations
        {
            public const string TicketsCollectionName = "_tickets";

            public static Expression<Func<TicketOwner, ICollection<CreatedTicket>>> CreatedTickets =>
                owner => owner._tickets;
        }
    }
}