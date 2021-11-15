using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.PromotionWaiters.BusinessRules
{
    internal sealed class ChangeAvailabilityRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeAvailabilityRules(PromotionWaiter promotionWaiter)
        {
            this.FollowingRules.Add(new AggregateMustExist(promotionWaiter.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(promotionWaiter.Status));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}