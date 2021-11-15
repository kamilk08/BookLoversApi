using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.ProfileManagers.Services;

namespace BookLovers.Readers.Domain.ProfileManagers.BusinessRules
{
    internal class PrivacyTypeMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid privacy type";
        private readonly ProfilePrivacyType _privacyType;

        public PrivacyTypeMustBeValid(ProfilePrivacyType privacyType)
        {
            _privacyType = privacyType;
        }

        public bool IsFulfilled()
        {
            return _privacyType != null &&
                   CurrentPrivacyOptions.CurrentOptions.ContainsKey(_privacyType);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}