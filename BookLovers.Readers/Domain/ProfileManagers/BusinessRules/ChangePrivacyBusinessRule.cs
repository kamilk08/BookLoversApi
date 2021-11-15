using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.BusinessRules
{
    internal sealed class ChangePrivacyBusinessRule : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangePrivacyBusinessRule(
            ProfilePrivacyManager manager,
            ProfilePrivacyType privacyType,
            PrivacyOption privacyOption)
        {
            FollowingRules.Add(new AggregateMustExist(manager.Guid));
            FollowingRules.Add(new AggregateMustBeActive(manager.AggregateStatus.Value));
            FollowingRules.Add(new PrivacyOptionMustBeValid(privacyOption));
            FollowingRules.Add(new PrivacyTypeMustBeValid(privacyType));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}