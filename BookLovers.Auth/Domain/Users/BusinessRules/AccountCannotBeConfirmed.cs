using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class AccountCannotBeConfirmed : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Account already confirmed.";

        private readonly Account _account;

        public AccountCannotBeConfirmed(Account account)
        {
            _account = account;
        }

        public bool IsFulfilled() => !_account.AccountConfirmation.IsConfirmed;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}