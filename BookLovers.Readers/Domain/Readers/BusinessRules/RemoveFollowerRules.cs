using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class RemoveFollowerRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public RemoveFollowerRules(Reader reader, Follower follower)
        {
            FollowingRules.Add(new AggregateMustExist(reader.Guid));
            FollowingRules.Add(new AggregateMustBeActive(reader.AggregateStatus != null
                ? reader.AggregateStatus.Value
                : AggregateStatus.Archived.Value));
            FollowingRules.Add(new ReaderMustContainSelectedFollower(reader, follower));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}