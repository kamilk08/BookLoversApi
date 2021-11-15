using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class RemoveReaderResourceRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public RemoveReaderResourceRules(Reader reader, IAddedResource addedResource)
        {
            FollowingRules.Add(new AggregateMustExist(reader.Guid));
            FollowingRules.Add(new AggregateMustBeActive(reader.AggregateStatus.Value));
            FollowingRules.Add(new AddedResourceMustBePresent(reader, addedResource));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}