using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserMustBeAReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "User must be a reader.";
        private readonly User _user;

        public UserMustBeAReader(User user)
        {
            _user = user;
        }

        public bool IsFulfilled()
        {
            return _user.IsInRole(Role.Reader.Name);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}