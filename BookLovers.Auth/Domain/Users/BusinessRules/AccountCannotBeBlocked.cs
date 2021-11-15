using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class AccountCannotBeBlockedRule : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Account cannot be blocked.";

        private readonly Account _account;

        public AccountCannotBeBlockedRule(Account account)
        {
            _account = account;
        }

        public bool IsFulfilled() => !_account.AccountSecurity.IsBlocked;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}