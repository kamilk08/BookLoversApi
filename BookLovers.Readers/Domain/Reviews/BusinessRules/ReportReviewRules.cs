using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class ReportReviewRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; }
            = new List<IBusinessRule>();

        public ReportReviewRules(Review review, ReviewReport report)
        {
            FollowingRules.Add(new AggregateMustExist(review.Guid));
            FollowingRules.Add(new AggregateMustBeActive(review.AggregateStatus != null
                ? review.AggregateStatus.Value
                : AggregateStatus.Archived.Value));
            FollowingRules.Add(new ReviewCannotBeReportedMultipleTimesBySameReader(review, report));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}