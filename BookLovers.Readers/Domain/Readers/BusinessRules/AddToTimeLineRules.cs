using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class AddToTimeLineRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public AddToTimeLineRules(Reader reader, Activity activity)
        {
            FollowingRules.Add(new AggregateMustExist(reader.Guid));
            FollowingRules.Add(new AggregateMustBeActive(reader.AggregateStatus.Value));
            FollowingRules.Add(new ActivityCannotBeAlreadyOnTimeLine(reader, activity));
            FollowingRules.Add(new ActivityTypeMustBeValid(activity));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}