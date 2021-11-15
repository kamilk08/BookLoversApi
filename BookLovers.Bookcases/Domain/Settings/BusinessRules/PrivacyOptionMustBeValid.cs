using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Domain.Settings.BusinessRules
{
    internal class PrivacyOptionMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Privacy option is invalid.";
        private readonly PrivacyOption _privacyOption;

        public PrivacyOptionMustBeValid(PrivacyOption privacyOption)
        {
            _privacyOption = privacyOption;
        }

        public bool IsFulfilled() => _privacyOption != null && AvailablePrivacyOptions.Has(_privacyOption);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}