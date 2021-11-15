using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.PublisherCycles.BusinessRules
{
    internal class CycleMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cycle is not unique.";

        private readonly IPublisherCycleUniquenessChecker _checker;
        private readonly Guid _guid;

        public CycleMustBeUnique(IPublisherCycleUniquenessChecker checker, Guid guid)
        {
            this._checker = checker;
            this._guid = guid;
        }

        public bool IsFulfilled()
        {
            return this._checker.IsUnique(this._guid);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}