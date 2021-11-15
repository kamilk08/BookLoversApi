using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.Services;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class TitleMustBeUnique : IBusinessRule
    {
        private const string BrokenBusinessRuleMessage = "Title must be unique";

        private readonly ITitleUniquenessChecker _uniquenessChecker;
        private readonly string _title;

        public TitleMustBeUnique(ITitleUniquenessChecker uniquenessChecker, string title)
        {
            this._uniquenessChecker = uniquenessChecker;
            this._title = title;
        }

        public bool IsFulfilled()
        {
            return this._uniquenessChecker != null
                   && this._uniquenessChecker.IsUnique(this._title);
        }

        public string BrokenRuleMessage => BrokenBusinessRuleMessage;
    }
}