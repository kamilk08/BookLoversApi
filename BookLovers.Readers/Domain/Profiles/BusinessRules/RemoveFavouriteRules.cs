using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class RemoveFavouriteRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public RemoveFavouriteRules(Profile profile, IFavourite favourite)
        {
            FollowingRules.Add(new AggregateMustExist(profile.Guid));
            FollowingRules.Add(new AggregateMustBeActive(profile.AggregateStatus.Value));
            FollowingRules.Add(new ProfileMustHaveSelectedFavourite(profile, favourite));
        }

        public bool IsFulfilled()
        {
            return !AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => Message;
    }
}