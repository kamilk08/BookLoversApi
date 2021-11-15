using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Librarians.Events.Tickets;

namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public class TicketManager : IAggregateManager<Ticket>
    {
        private readonly List<Func<Ticket, IBusinessRule>> _rules = new List<Func<Ticket, IBusinessRule>>();

        public TicketManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.Status));
        }

        public void Archive(Ticket aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ArchiveAggregate();

            aggregate.AddEvent(new TicketArchived(aggregate.Guid));
        }
    }
}