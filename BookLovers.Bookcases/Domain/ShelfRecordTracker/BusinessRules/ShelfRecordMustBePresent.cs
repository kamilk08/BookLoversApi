using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal class ShelfRecordMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf record is missing";

        private readonly ShelfRecordTracker _tracker;
        private readonly ShelfRecord _shelfRecord;

        public ShelfRecordMustBePresent(
            ShelfRecordTracker tracker,
            ShelfRecord shelfRecord)
        {
            _tracker = tracker;
            _shelfRecord = shelfRecord;
        }

        public bool IsFulfilled() =>
            _tracker.ShelfRecords.Contains(_shelfRecord);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}