using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.IsbnValidation;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class IsbnNumberMustBeValid : IBusinessRule
    {
        private const string BrokenBusinessRuleMessage = "Invalid book ISBN number";

        private readonly IIsbnValidator _isbnValidator;
        private readonly string _bookIsbn;

        public IsbnNumberMustBeValid(IIsbnValidator isbnValidator, string bookIsbn)
        {
            this._isbnValidator = isbnValidator;
            this._bookIsbn = bookIsbn;
        }

        public bool IsFulfilled()
        {
            return this._isbnValidator != null
                   && this._isbnValidator.IsIsbnValid(this._bookIsbn);
        }

        public string BrokenRuleMessage => BrokenBusinessRuleMessage;
    }
}