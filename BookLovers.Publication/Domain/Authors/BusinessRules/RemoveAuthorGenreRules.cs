using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal sealed class RemoveAuthorGenreRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveAuthorGenreRules(Author author, SubCategory subCategory)
        {
            this.FollowingRules.Add(new AggregateMustExist(author.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(author.AggregateStatus.Value));
            this.FollowingRules.Add(new ValidAuthorGenre(subCategory));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}