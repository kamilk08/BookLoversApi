using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.PublisherCycles.BusinessRules
{
    internal class PublisherCycleMustHaveSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher cycle does not have selected book.";

        private readonly PublisherCycle _publisherCycle;
        private readonly Book _book;

        public PublisherCycleMustHaveSelectedBook(PublisherCycle publisherCycle, Book book)
        {
            this._publisherCycle = publisherCycle;
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._publisherCycle.Books.Contains(this._book);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}