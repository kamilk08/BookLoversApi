using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules
{
    internal class RegistrationCannotBeCompleted : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Registration cannot be completed.";

        private readonly RegistrationCompletion _registrationCompletion;

        public RegistrationCannotBeCompleted(RegistrationCompletion registrationCompletion)
        {
            _registrationCompletion = registrationCompletion;
        }

        public bool IsFulfilled()
        {
            return !_registrationCompletion.CompletedAt.HasValue &&
                   _registrationCompletion.CompletedAt != default(DateTime);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}