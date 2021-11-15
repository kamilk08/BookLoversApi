using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.PublisherCycles.BusinessRules;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Domain.PublisherCycles
{
    public class PublisherCycleFactory
    {
        private readonly List<Func<PublisherCycle, Publisher, IBusinessRule>> _rules =
            new List<Func<PublisherCycle, Publisher, IBusinessRule>>();

        public PublisherCycleFactory(IPublisherCycleUniquenessChecker uniquenessChecker)
        {
            this._rules.Add((cycle, publisher) => new AggregateMustBeActive(cycle.AggregateStatus.Value));
            this._rules.Add((cycle, publisher) => new CycleMustHavePublisher(cycle));
            this._rules.Add((cycle, publisher) => new CycleMustBeUnique(uniquenessChecker, cycle.Guid));
        }

        public PublisherCycle Create(
            Guid cycleGuid,
            Publisher publisher,
            string cycleName)
        {
            var publisherCycle = new PublisherCycle(cycleGuid, publisher.Guid, cycleName);

            foreach (var rule in this._rules)
            {
                if (!rule(publisherCycle, publisher).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(publisherCycle, publisher).BrokenRuleMessage);
            }

            return publisherCycle;
        }
    }
}