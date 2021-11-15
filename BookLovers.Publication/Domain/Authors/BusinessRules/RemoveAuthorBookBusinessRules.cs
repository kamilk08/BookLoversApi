using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal sealed class RemoveAuthorBookBusinessRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveAuthorBookBusinessRules(Author author, AuthorBook authorBook)
        {
            this.FollowingRules.Add(new AggregateMustExist(author.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(author.AggregateStatus.Value));
            this.FollowingRules.Add(new AuthorMustHaveBookInHisCollection(author, authorBook));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}