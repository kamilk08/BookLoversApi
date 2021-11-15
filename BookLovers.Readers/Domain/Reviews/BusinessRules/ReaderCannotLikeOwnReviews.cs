using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReaderCannotLikeOwnReviews : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader cannot like own reviews.";

        private readonly Review _review;
        private readonly Guid _readerGuid;

        public ReaderCannotLikeOwnReviews(Review review, Guid readerGuid)
        {
            _review = review;
            _readerGuid = readerGuid;
        }

        public bool IsFulfilled()
        {
            return _review.ReviewIdentification.ReaderGuid != _readerGuid;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}