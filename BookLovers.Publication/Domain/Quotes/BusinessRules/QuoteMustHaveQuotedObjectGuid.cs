using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class QuoteMustHaveQuotedObjectGuid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Quote must have qouted object guid.";

        private readonly Quote _quote;

        public QuoteMustHaveQuotedObjectGuid(Quote quote)
        {
            this._quote = quote;
        }

        public bool IsFulfilled()
        {
            return this._quote.QuoteContent.QuotedGuid != Guid.Empty;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}