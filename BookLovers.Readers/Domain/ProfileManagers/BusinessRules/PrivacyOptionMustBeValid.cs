using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.BusinessRules
{
    internal class PrivacyOptionMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid privacy option.";
        private readonly PrivacyOption _option;

        public PrivacyOptionMustBeValid(PrivacyOption option)
        {
            _option = option;
        }

        public bool IsFulfilled()
        {
            return _option != null && AvailablePrivacyOptions.Has(_option);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}