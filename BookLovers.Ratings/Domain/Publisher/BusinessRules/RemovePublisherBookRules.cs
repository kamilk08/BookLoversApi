using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class RemovePublisherBookRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RemovePublisherBookRules(Publisher publisher, Book book)
        {
            FollowingRules.Add(new AggregateMustExist(book.Guid));
            FollowingRules.Add(new AggregateMustExist(publisher.Guid));
            FollowingRules.Add(new PublisherMustHaveSelectedBook(publisher, book));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}