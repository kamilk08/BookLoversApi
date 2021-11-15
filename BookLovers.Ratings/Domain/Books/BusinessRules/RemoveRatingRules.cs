using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class RemoveRatingRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveRatingRules(Book book, Rating rating)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new RemovingNotExistedRatingIsNotAllowed(book, rating));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}