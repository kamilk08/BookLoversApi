using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    public class PublisherMustBeAvailable : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher does not exist";

        private readonly Publisher _publisher;

        public PublisherMustBeAvailable(Publisher publisher)
        {
            this._publisher = publisher;
        }

        public bool IsFulfilled()
        {
            return this._publisher.IsActive();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}