using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReviewCannotBeReportedMultipleTimesBySameReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review has been already reported by reader.";

        private readonly Review _review;
        private readonly ReviewReport _reviewReport;

        public ReviewCannotBeReportedMultipleTimesBySameReader(Review review, ReviewReport reviewReport)
        {
            _review = review;
            _reviewReport = reviewReport;
        }

        public bool IsFulfilled() => !_review.Reports.Contains(_reviewReport);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}