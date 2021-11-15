using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class RemoveCycleRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveCycleRules(Publisher publisher, PublisherCycle publisherCycle)
        {
            FollowingRules.Add(new AggregateMustExist(publisherCycle.Guid));
            FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            FollowingRules.Add(new PublisherMustContainSelectedCycle(publisher, publisherCycle));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}