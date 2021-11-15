using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal sealed class ReTrackBookRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ReTrackBookRules(
            ShelfRecordTracker tracker,
            ShelfRecord oldShelfRecord,
            ShelfRecord newShelfRecord)
        {
            FollowingRules.Add(new AggregateMustExist(tracker.Guid));
            FollowingRules.Add(new AggregateMustBeActive(tracker.AggregateStatus.Value));
            FollowingRules.Add(new ReTrackBookMustBeTheSame(oldShelfRecord, newShelfRecord));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}