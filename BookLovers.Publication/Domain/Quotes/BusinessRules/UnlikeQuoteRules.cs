using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Likes;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class UnlikeQuoteRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public UnlikeQuoteRules(Quote quote, Like like)
        {
            this.FollowingRules.Add(new AggregateMustExist(quote.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(quote.AggregateStatus != null
                ? quote.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            this.FollowingRules.Add(new QuoteMustContainSelectedLike(quote, like));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}