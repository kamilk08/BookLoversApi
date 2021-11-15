using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class AddBookToPublisherCycleRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddBookToPublisherCycleRules(PublisherCycle publisherCycle, CycleBook cycleBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(publisherCycle.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisherCycle.AggregateStatus.Value));
            this.FollowingRules.Add(new PublisherCycleBookCannotBeInvalid(cycleBook));
            this.FollowingRules.Add(new PublisherCycleCannotHaveDuplicatedBooks(publisherCycle, cycleBook.BookGuid));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}