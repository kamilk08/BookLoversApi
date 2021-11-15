using System;
using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class PublisherCycleCannotHaveDuplicatedBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cannot add duplicated book to publisher cycle.";

        private readonly PublisherCycle _publisherCycle;
        private readonly Guid _bookGuid;

        public PublisherCycleCannotHaveDuplicatedBooks(PublisherCycle publisherCycle, Guid bookGuid)
        {
            this._publisherCycle = publisherCycle;
            this._bookGuid = bookGuid;
        }

        public bool IsFulfilled()
        {
            return !this._publisherCycle.Books.Any(a => a.BookGuid
                                                        == this._bookGuid);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}