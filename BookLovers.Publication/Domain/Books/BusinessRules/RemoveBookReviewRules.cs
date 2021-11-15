using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class RemoveBookReviewRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemoveBookReviewRules(Book book, BookReview bookReview)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(book.AggregateStatus.Value));
            this.FollowingRules.Add(new BookMustHaveSelectedReview(book, bookReview));
        }

        public bool IsFulfilled()
        {
            return !this.AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => this.Message;
    }
}