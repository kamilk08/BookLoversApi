using System;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Quotes.Services;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class QuoteMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Quote is not unique";

        private readonly IQuoteUniquenessChecker _checker;
        private readonly Guid _guid;

        public QuoteMustBeUnique(IQuoteUniquenessChecker checker, Guid guid)
        {
            this._checker = checker;
            this._guid = guid;
        }

        public bool IsFulfilled() => this._checker.IsUnique(this._guid);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}