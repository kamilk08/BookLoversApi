using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.Quotes;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    public class QuoteManager : IAggregateManager<Quote>
    {
        private readonly IList<Func<Quote, IBusinessRule>> _rules;

        public QuoteManager()
        {
            this._rules = new List<Func<Quote, IBusinessRule>>
            {
                aggregate => new AggregateMustExist(aggregate?.Guid ?? Guid.Empty),
                aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value)
            };
        }

        public void Archive(Quote aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new QuoteArchived(aggregate.Guid, aggregate.QuoteContent.QuotedGuid,
                aggregate.QuoteDetails.AddedByGuid));
        }
    }
}