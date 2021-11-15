using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class ChangeIdentityRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeIdentityRules(Profile profile, Sex sex)
        {
            FollowingRules.Add(new AggregateMustExist(profile.Guid));
            FollowingRules.Add(new AggregateMustBeActive(profile.AggregateStatus.Value));
            FollowingRules.Add(new SexTypeMustBeValid(sex));
        }

        public bool IsFulfilled()
        {
            return !AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => Message;
    }
}