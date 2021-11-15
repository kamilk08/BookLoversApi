using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class PublisherCycleMustContainSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher cycle must contain selected book.";

        private readonly PublisherCycle _publisherCycle;
        private readonly CycleBook _cycleBook;

        public PublisherCycleMustContainSelectedBook(PublisherCycle publisherCycle, CycleBook cycleBook)
        {
            this._publisherCycle = publisherCycle;
            this._cycleBook = cycleBook;
        }

        public bool IsFulfilled()
        {
            return this._publisherCycle.Books.Contains(this._cycleBook);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}