using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class AccountMustBeConfirmed : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Account must be confirmed.";

        private readonly Account _account;

        public AccountMustBeConfirmed(Account account)
        {
            _account = account;
        }

        public bool IsFulfilled() => _account.AccountConfirmation.IsConfirmed;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}