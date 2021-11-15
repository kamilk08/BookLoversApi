using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class PublisherCannotHaveDuplicatedCycles : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher cannot have duplicated cycles.";

        private readonly Publisher _publisher;
        private readonly PublisherCycle _publisherCycle;

        public PublisherCannotHaveDuplicatedCycles(
            Publisher publisher,
            PublisherCycle publisherCycle)
        {
            _publisher = publisher;
            _publisherCycle = publisherCycle;
        }

        public bool IsFulfilled()
        {
            return !_publisher.PublisherCycles.Contains(_publisherCycle);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}