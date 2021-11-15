using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal class ReTrackBookMustBeTheSame : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Retracked book must be the same";

        private readonly ShelfRecord _oldShelfRecord;
        private readonly ShelfRecord _newShelfRecord;

        public ReTrackBookMustBeTheSame(ShelfRecord oldShelfRecord, ShelfRecord newShelfRecord)
        {
            _oldShelfRecord = oldShelfRecord;
            _newShelfRecord = newShelfRecord;
        }

        public bool IsFulfilled() => _oldShelfRecord.BookGuid == _newShelfRecord.BookGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}