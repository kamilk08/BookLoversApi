using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Domain.RatingGivers
{
    internal class RatingGiverManager : IAggregateManager<RatingGiver>
    {
        private readonly List<Func<RatingGiver, IBusinessRule>> _rules =
            new List<Func<RatingGiver, IBusinessRule>>();

        public RatingGiverManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.Status));
        }

        public void Archive(RatingGiver aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ArchiveAggregate();

            aggregate.AddEvent(new RatingGiverArchived(aggregate.ReaderGuid, aggregate.ReaderId,
                aggregate.Ratings.Select(s => s.BookId).ToList()));
        }
    }
}