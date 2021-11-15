using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules
{
    internal class RegistrationTokensMustBeEqual : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Registration tokens must be equal";

        private readonly RegistrationSummary _registrationSummary;
        private readonly string _token;

        public RegistrationTokensMustBeEqual(RegistrationSummary registrationSummary, string token)
        {
            _registrationSummary = registrationSummary;
            _token = token;
        }

        public bool IsFulfilled() => _registrationSummary.AreTokensEqual(_token);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}