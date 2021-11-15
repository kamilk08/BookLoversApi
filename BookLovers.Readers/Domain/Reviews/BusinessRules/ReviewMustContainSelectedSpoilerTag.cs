using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewMustContainSelectedSpoilerTag : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review does not have spoiler tag from selected reader.";

        private readonly Review _review;
        private readonly SpoilerTag _spoilerTag;

        public ReviewMustContainSelectedSpoilerTag(Review review, SpoilerTag spoilerTag)
        {
            _review = review;
            _spoilerTag = spoilerTag;
        }

        public bool IsFulfilled() => _review.SpoilerTags.Contains(_spoilerTag);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}