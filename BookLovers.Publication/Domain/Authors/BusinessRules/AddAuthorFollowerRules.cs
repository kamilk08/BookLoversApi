using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal sealed class AddAuthorFollowerRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public IReadOnlyList<IBusinessRule> Rules => this.FollowingRules;

        public AddAuthorFollowerRules(Author author, Follower follower)
        {
            this.FollowingRules.Add(new AggregateMustExist(author.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(author.AggregateStatus != null
                ? author.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            this.FollowingRules.Add(new AuthorCannotHaveDuplicatedFollowers(author, follower));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}