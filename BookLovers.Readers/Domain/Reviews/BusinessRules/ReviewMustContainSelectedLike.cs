using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Likes;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewMustContainSelectedLike : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review has not been liked by selected reader.";

        private readonly Review _review;
        private readonly Like _like;

        public ReviewMustContainSelectedLike(Review review, Like like)
        {
            _review = review;
            _like = like;
        }

        public bool IsFulfilled() => _review.Likes.Contains(_like);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}