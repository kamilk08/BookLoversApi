using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class AddPublisherBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddPublisherBookRules(Publisher publisher, PublisherBook publisherBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(publisher.AggregateStatus.Value));
            this.FollowingRules.Add(new PublisherCannotHaveDuplicatedBooks(publisher, publisherBook));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}