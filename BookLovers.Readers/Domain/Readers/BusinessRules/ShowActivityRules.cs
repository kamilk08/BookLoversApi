using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ShowActivityRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public ShowActivityRules(Reader reader, Activity activity)
        {
            FollowingRules.Add(new AggregateMustExist(reader.Guid));
            FollowingRules.Add(new AggregateMustBeActive(reader.AggregateStatus != null
                ? reader.AggregateStatus.Value
                : AggregateStatus.Archived.Value));
            FollowingRules.Add(new ActivityMustBeOnTimeLine(reader, activity));
            FollowingRules.Add(new ActivityMustBeHidden(activity));
            FollowingRules.Add(new ActivityTypeMustBeValid(activity));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}