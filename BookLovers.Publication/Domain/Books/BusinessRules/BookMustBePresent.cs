using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book does not exist.";
        private readonly Book _book;

        public BookMustBePresent(Book book)
        {
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._book.IsActive();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}