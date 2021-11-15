using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal sealed class TrackBookRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public TrackBookRules(ShelfRecordTracker tracker, ShelfRecord record)
        {
            FollowingRules.Add(new AggregateMustExist(tracker.Guid));
            FollowingRules.Add(new AggregateMustBeActive(tracker.AggregateStatus.Value));
            FollowingRules.Add(new ShelfRecordTrackerShouldNotContainDuplicatedRecords(tracker, record));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}