using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class UserNameMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Username already taken.";

        private readonly IUserNameUniquenessChecker _uniquenessChecker;
        private readonly string _userName;

        public UserNameMustBeUnique(IUserNameUniquenessChecker uniquenessChecker, string userName)
        {
            _uniquenessChecker = uniquenessChecker;
            _userName = userName;
        }

        public bool IsFulfilled() => _uniquenessChecker.IsUserNameUnique(_userName);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}