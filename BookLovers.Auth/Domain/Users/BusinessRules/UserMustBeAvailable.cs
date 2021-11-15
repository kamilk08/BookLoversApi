using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserMustBeAvailable : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "User does not exist.";
        private readonly User _user;

        public UserMustBeAvailable(User user)
        {
            _user = user;
        }

        public bool IsFulfilled() => _user != null;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}