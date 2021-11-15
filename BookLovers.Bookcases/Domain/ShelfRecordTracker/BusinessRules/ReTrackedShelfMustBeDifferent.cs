using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal class ReTrackedShelfMustBeDifferent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Retracked shelf must be different";

        private readonly ShelfRecord _oldShelfRecord;
        private readonly ShelfRecord _newShelfRecord;

        public ReTrackedShelfMustBeDifferent(ShelfRecord oldShelfRecord, ShelfRecord newShelfRecord)
        {
            _oldShelfRecord = oldShelfRecord;
            _newShelfRecord = newShelfRecord;
        }

        public bool IsFulfilled() => _oldShelfRecord.ShelfGuid != _newShelfRecord.ShelfGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}