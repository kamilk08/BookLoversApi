using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules
{
    internal sealed class FinishRegistrationWithoutCompletionRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public FinishRegistrationWithoutCompletionRules(RegistrationSummary summary)
        {
            FollowingRules.Add(new AggregateMustExist(summary.Guid));
            FollowingRules.Add(new AggregateMustBeActive(summary.Status));
            FollowingRules.Add(new RegistrationCannotBeCompleted(summary.Completion));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}