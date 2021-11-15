using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserCannotHaveZeroRoles : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "User must have at least one role.";

        private readonly User _user;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;

        public UserCannotHaveZeroRoles(User user)
        {
            _user = user;
        }

        public bool IsFulfilled()
        {
            return UserHaveMultipleRoles(_user);
        }

        private bool UserHaveMultipleRoles(User user)
        {
            return user.UserRoles.Count > 1;
        }
    }
}