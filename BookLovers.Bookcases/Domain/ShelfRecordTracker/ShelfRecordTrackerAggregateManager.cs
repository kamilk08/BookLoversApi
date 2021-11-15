using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Events.ShelfRecordTracker;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker
{
    internal class
        ShelfRecordTrackerAggregateManager : IAggregateManager<
            ShelfRecordTracker>
    {
        private readonly List<Func<ShelfRecordTracker, IBusinessRule>>
            _rules;

        public ShelfRecordTrackerAggregateManager()
        {
            _rules = new List<Func<ShelfRecordTracker, IBusinessRule>>();

            _rules.Add(shelfRecordTracker => new AggregateMustExist(shelfRecordTracker.Guid));
            _rules.Add(shelfRecordTracker => new AggregateMustBeActive(shelfRecordTracker.AggregateStatus.Value));
        }

        public void Archive(ShelfRecordTracker aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new ShelfRecordTrackerArchived(aggregate.Guid));
        }
    }
}