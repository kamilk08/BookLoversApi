using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Authors.BusinessRules
{
    internal class RemoveAuthorBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; }
            = new List<IBusinessRule>();

        public RemoveAuthorBookRules(Author author, Book book)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustExist(author.Guid));
            this.FollowingRules.Add(new AuthorMustHaveBook(author, book));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}