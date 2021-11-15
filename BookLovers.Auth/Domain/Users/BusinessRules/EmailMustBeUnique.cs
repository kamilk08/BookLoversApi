using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal class EmailMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Email is not unique.";

        private readonly IEmailUniquenessChecker _emailUniquenessChecker;
        private readonly string _email;

        public EmailMustBeUnique(IEmailUniquenessChecker emailUniquenessChecker, string email)
        {
            _emailUniquenessChecker = emailUniquenessChecker;
            _email = email;
        }

        public bool IsFulfilled() => _emailUniquenessChecker.IsEmailUnique(_email);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}