using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.Languages;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class LanguageTypeMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid language type.";

        private readonly Language _language;

        public LanguageTypeMustBeValid(Language language)
        {
            this._language = language;
        }

        public bool IsFulfilled()
        {
            return BookLanguages.Has(this._language);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}