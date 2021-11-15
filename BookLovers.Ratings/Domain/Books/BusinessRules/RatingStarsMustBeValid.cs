using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.RatingStars;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class RatingStarsMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Rating stars is not valid.";

        private readonly int _stars;

        public RatingStarsMustBeValid(int stars)
        {
            this._stars = stars;
        }

        public bool IsFulfilled()
        {
            return StarList.Stars.Any(s => s.Value == this._stars);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}