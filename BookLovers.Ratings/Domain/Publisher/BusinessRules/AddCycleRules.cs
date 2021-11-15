using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class AddCycleRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddCycleRules(Publisher publisher, PublisherCycle cycle)
        {
            FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            FollowingRules.Add(new AggregateMustExist(cycle.Guid));
            FollowingRules.Add(new AggregateMustBeActive(publisher.Status));
            FollowingRules.Add(new PublisherCannotHaveDuplicatedCycles(publisher, cycle));
        }

        public bool IsFulfilled()
        {
            return !AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => Message;
    }
}