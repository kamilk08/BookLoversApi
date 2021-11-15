using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class AddPublisherBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddPublisherBookRules(Publisher publisher, Book book)
        {
            FollowingRules.Add(new AggregateMustExist(book.Guid));
            FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            FollowingRules.Add(new AggregateMustBeActive(publisher.Status));
            FollowingRules.Add(new PublisherCannotHaveDuplicatedBooks(publisher, book));
        }

        public bool IsFulfilled()
        {
            return !AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => Message;
    }
}