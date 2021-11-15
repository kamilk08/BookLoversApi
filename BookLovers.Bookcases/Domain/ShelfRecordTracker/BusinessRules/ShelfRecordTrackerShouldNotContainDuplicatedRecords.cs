using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal class ShelfRecordTrackerShouldNotContainDuplicatedRecords : IBusinessRule
    {
        private const string BrokenBusinessRuleMessage = "Shelf record tracker should not contain duplicated records";

        private readonly ShelfRecordTracker _tracker;
        private readonly ShelfRecord _record;

        public ShelfRecordTrackerShouldNotContainDuplicatedRecords(ShelfRecordTracker tracker, ShelfRecord record)
        {
            _tracker = tracker;
            _record = record;
        }

        public bool IsFulfilled()
        {
            return _tracker.GetTrackedBook(_record.BookGuid, _record.ShelfGuid) == null;
        }

        public string BrokenRuleMessage => BrokenBusinessRuleMessage;
    }
}