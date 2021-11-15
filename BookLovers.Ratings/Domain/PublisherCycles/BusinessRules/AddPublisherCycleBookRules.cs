using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.PublisherCycles.BusinessRules
{
    internal class AddPublisherCycleBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddPublisherCycleBookRules(PublisherCycle publisherCycle, Book book)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustExist(publisherCycle.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisherCycle.Status));
            this.FollowingRules.Add(new PublisherCycleCannotHaveDuplicatedBooks(publisherCycle, book));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}