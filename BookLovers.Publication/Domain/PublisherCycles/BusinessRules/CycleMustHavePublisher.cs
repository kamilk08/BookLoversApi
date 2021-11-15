using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class CycleMustHavePublisher : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cycle must have publisher.";

        private readonly PublisherCycle _publisherCycle;

        public CycleMustHavePublisher(PublisherCycle publisherCycle)
        {
            this._publisherCycle = publisherCycle;
        }

        public bool IsFulfilled()
        {
            return this._publisherCycle.PublisherGuid != Guid.Empty;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}