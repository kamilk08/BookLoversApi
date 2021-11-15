using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReaderCannotAddSpoilerTagToOwnReview : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader cannot add spoiler tag to own review.";

        private readonly Review _review;
        private readonly SpoilerTag _spoilerTag;

        public ReaderCannotAddSpoilerTagToOwnReview(Review review, SpoilerTag spoilerTag)
        {
            _review = review;
            _spoilerTag = spoilerTag;
        }

        public bool IsFulfilled() =>
            _review.ReviewIdentification.ReaderGuid != _spoilerTag.ReaderGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}