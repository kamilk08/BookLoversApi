using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Domain.Reviews.Services
{
    internal class ReviewAggregateManager : IAggregateManager<Review>
    {
        private readonly List<Func<Review, IBusinessRule>> _rules =
            new List<Func<Review, IBusinessRule>>();

        public ReviewAggregateManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate==null ? Guid.Empty : aggregate.Guid));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(Review aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new ReviewArchived(aggregate.Guid, aggregate.ReviewIdentification.ReaderGuid,
                aggregate.ReviewIdentification.BookGuid));
        }
    }
}