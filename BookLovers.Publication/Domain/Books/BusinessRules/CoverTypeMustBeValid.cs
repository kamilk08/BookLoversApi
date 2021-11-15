using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.CoverTypes;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class CoverTypeMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book cover type is invalid";

        private readonly CoverType _coverType;

        public CoverTypeMustBeValid(CoverType coverType)
        {
            this._coverType = coverType;
        }

        public bool IsFulfilled()
        {
            return BookCovers.Has(this._coverType);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}