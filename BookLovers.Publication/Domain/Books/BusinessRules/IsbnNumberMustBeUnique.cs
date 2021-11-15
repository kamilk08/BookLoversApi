using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.Services;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class IsbnNumberMustBeUnique : IBusinessRule
    {
        private const string BrokenBusinessRuleMessage = "ISBN number is not unique.";

        private readonly IIsbnUniquenessChecker _isbnUniquenessChecker;
        private readonly string _isbnNumber;

        public IsbnNumberMustBeUnique(
            IIsbnUniquenessChecker isbnUniquenessChecker,
            string isbnNumber)
        {
            this._isbnUniquenessChecker = isbnUniquenessChecker;
            this._isbnNumber = isbnNumber;
        }

        public bool IsFulfilled()
        {
            return this._isbnUniquenessChecker != null &&
                   this._isbnUniquenessChecker.IsUnique(this._isbnNumber);
        }

        public string BrokenRuleMessage => BrokenBusinessRuleMessage;
    }
}