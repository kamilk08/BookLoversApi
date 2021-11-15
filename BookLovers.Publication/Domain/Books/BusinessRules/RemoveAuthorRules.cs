using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class RemoveAuthorRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveAuthorRules(Book book, BookAuthor bookAuthor)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(book.AggregateStatus.Value));
            this.FollowingRules.Add(new BookMustHaveSelectedAuthor(book, bookAuthor));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}