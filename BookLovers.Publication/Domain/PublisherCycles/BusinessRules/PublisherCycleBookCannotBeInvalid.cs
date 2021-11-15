using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class PublisherCycleBookCannotBeInvalid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid publisher cycle book.";

        private readonly CycleBook _cycleBook;

        public PublisherCycleBookCannotBeInvalid(CycleBook cycleBook)
        {
            this._cycleBook = cycleBook;
        }

        public bool IsFulfilled()
        {
            return this._cycleBook.BookGuid != Guid.Empty;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}