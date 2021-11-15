using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class AddPublisherCycleRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddPublisherCycleRules(Publisher publisher, Cycle cycle)
        {
            this.FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisher.AggregateStatus.Value));
            this.FollowingRules.Add(new PublisherCannotHaveDuplicatedCycles(publisher, cycle));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}