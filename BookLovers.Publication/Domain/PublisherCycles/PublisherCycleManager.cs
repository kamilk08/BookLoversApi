using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.PublisherCycles;

namespace BookLovers.Publication.Domain.PublisherCycles
{
    public class PublisherCycleManager : IAggregateManager<PublisherCycle>
    {
        private readonly List<Func<PublisherCycle, IBusinessRule>> _rules =
            new List<Func<PublisherCycle, IBusinessRule>>();

        public PublisherCycleManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(PublisherCycle aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var @event = new PublisherCycleArchived(aggregate.Guid, aggregate.PublisherGuid);

            aggregate.ApplyChange(@event);
        }
    }
}