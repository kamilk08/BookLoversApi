using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class PublisherMustContainSelectedCycle : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher does not have selected cycle.";

        private readonly Publisher _publisher;
        private readonly Cycle _cycle;

        public PublisherMustContainSelectedCycle(Publisher publisher, Cycle cycle)
        {
            this._publisher = publisher;
            this._cycle = cycle;
        }

        public bool IsFulfilled() =>
            this._publisher.Cycles.Contains(this._cycle);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}