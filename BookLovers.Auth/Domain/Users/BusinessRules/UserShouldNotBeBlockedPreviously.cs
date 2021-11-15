using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserShouldNotBeBlockedPreviously : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "User has been a bad person :(";

        private readonly Account _account;

        public UserShouldNotBeBlockedPreviously(Account account)
        {
            _account = account;
        }

        public bool IsFulfilled() => !_account.AccountDetails.HasBeenBlockedPreviously;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}