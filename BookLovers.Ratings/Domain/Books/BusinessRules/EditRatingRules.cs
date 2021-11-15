using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class EditRatingRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public EditRatingRules(Book book, RatingGiver ratingGiver, Rating newRating)
        {
            this.FollowingRules.Add(new AggregateMustExist(book.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(book.Status));
            this.FollowingRules.Add(new BookMustHaveRatingFromSelectedReader(book, ratingGiver));
            this.FollowingRules.Add(new RatingStarsMustBeValid(newRating.Stars));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}