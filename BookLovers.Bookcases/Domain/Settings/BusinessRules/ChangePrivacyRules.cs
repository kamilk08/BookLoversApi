using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Domain.Settings.BusinessRules
{
    internal sealed class ChangePrivacyRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangePrivacyRules(SettingsManager manager, PrivacyOption privacyOption)
        {
            FollowingRules.Add(new PrivacyOptionMustBeValid(privacyOption));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}