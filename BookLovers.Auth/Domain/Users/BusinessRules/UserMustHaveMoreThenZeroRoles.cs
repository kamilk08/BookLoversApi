using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserMustHaveMoreThenZeroRoles : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "User must have at least one role.";

        private readonly User _user;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;

        public UserMustHaveMoreThenZeroRoles(User user)
        {
            _user = user;
        }

        public bool IsFulfilled() => UserHaveMultipleRoles(_user);

        private bool UserHaveMultipleRoles(User user) => user.UserRoles.Count > 1;
    }
}