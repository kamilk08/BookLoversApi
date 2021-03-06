using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class HideActivityRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public HideActivityRules(Reader reader, Activity activity)
        {
            FollowingRules.Add(new AggregateMustExist(reader.Guid));
            FollowingRules.Add(new AggregateMustBeActive(reader.AggregateStatus != null
                ? reader.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            FollowingRules.Add(new ActivityMustBeOnTimeLine(reader, activity));
            FollowingRules.Add(new ActivityTypeMustBeValid(activity));
            FollowingRules.Add(new ActivityMustBeVisible(activity));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}