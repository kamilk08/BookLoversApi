using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author does not exist.";

        private readonly Author _author;

        public AuthorMustBePresent(Author author)
        {
            this._author = author;
        }

        public bool IsFulfilled() => this._author.IsActive();

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}