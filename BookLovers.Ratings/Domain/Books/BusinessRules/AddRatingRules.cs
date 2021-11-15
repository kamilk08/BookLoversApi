using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class AddRatingRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AddRatingRules(Book book, Rating rating)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(book.Status));
            this.FollowingRules.Add(new MultipleRatingsFromSameReaderAreNotAllowed(book, rating));
            this.FollowingRules.Add(new RatingStarsMustBeValid(rating.Stars));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}