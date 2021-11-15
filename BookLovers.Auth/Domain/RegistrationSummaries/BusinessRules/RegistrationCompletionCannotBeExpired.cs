using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules
{
    internal class RegistrationCompletionCannotBeExpired : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Registration completion has expired.";

        private readonly RegistrationSummary _registrationSummary;

        public RegistrationCompletionCannotBeExpired(RegistrationSummary registrationSummary)
        {
            _registrationSummary = registrationSummary;
        }

        public bool IsFulfilled() => !_registrationSummary.IsExpired();

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}