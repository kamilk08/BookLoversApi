using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Librarians.Events.TicketOwners;

namespace BookLovers.Librarians.Domain.TicketOwners
{
    internal class TicketOwnerManager : IAggregateManager<TicketOwner>
    {
        private readonly List<Func<TicketOwner, IBusinessRule>> _rules =
            new List<Func<TicketOwner, IBusinessRule>>();

        public TicketOwnerManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate?.Guid ?? Guid.Empty));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.Status));
        }

        public void Archive(TicketOwner aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ArchiveAggregate();

            aggregate.AddEvent(new TicketOwnerArchived(
                aggregate.Guid,
                aggregate.GetAllPendingTickets().Select(s => s.TicketGuid)));
        }
    }
}