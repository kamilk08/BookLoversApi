using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class OnlyReviewAuthorCanEditReview : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Only author of review can edit it.";

        private readonly Review _review;
        private readonly Guid _readerGuid;

        public OnlyReviewAuthorCanEditReview(Review review, Guid readerGuid)
        {
            _review = review;
            _readerGuid = readerGuid;
        }

        public bool IsFulfilled() =>
            _review.ReviewIdentification.ReaderGuid == _readerGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}