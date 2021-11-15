using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
    internal sealed class UnTrackBookRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public UnTrackBookRules(ShelfRecordTracker tracker, ShelfRecord shelfRecord)
        {
            FollowingRules.Add(new AggregateMustExist(tracker.Guid));
            FollowingRules.Add(new AggregateMustBeActive(tracker.AggregateStatus.Value));
            FollowingRules.Add(new ShelfRecordMustBePresent(tracker, shelfRecord));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}