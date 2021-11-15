using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewCannotHaveMultipleSpoilerTagsFromSameReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review cannot have multiple spoiler tags from same reader.";

        private readonly Review _review;
        private readonly SpoilerTag _spoilerTag;

        public ReviewCannotHaveMultipleSpoilerTagsFromSameReader(Review review, SpoilerTag spoilerTag)
        {
            _review = review;
            _spoilerTag = spoilerTag;
        }

        public bool IsFulfilled() => !_review.SpoilerTags.Contains(_spoilerTag);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}