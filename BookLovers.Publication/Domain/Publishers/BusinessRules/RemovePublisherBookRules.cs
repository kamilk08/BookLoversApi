using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class RemovePublisherBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemovePublisherBookRules(Publisher publisher, PublisherBook publisherBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisher.AggregateStatus.Value));
            this.FollowingRules.Add(new PublisherMustContainSelectedBook(publisher, publisherBook));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}