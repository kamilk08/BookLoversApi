using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Authors.BusinessRules
{
    internal class AuthorBookCannotBeNull : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author book cannot be null";

        private readonly Book _book;

        public AuthorBookCannotBeNull(Book book)
        {
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._book != null;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}