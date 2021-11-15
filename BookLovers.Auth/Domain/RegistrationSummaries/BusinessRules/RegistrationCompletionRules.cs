using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules
{
    internal sealed class RegistrationCompletionRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RegistrationCompletionRules(RegistrationSummary summary, string token)
        {
            FollowingRules.Add(new AggregateMustBeActive(summary.Status));
            FollowingRules.Add(new RegistrationCannotBeCompleted(summary.Completion));
            FollowingRules.Add(new RegistrationCompletionCannotBeExpired(summary));
            FollowingRules.Add(new RegistrationTokensMustBeEqual(summary, token));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}