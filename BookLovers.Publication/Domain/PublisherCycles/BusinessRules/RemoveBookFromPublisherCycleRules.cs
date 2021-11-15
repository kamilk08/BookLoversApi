using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class RemoveBookFromPublisherCycleRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveBookFromPublisherCycleRules(PublisherCycle publisherCycle, CycleBook cycleBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(publisherCycle.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisherCycle.AggregateStatus != null
                ? publisherCycle.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            this.FollowingRules.Add(new PublisherCycleMustContainSelectedBook(publisherCycle, cycleBook));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}