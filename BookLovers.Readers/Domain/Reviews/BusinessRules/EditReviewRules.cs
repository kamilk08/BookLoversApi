using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class EditReviewRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public EditReviewRules(Review review, Guid readerGuid)
        {
            FollowingRules.Add(new AggregateMustExist(review.Guid));
            FollowingRules.Add(new AggregateMustBeActive(review.AggregateStatus != null
                ? review.AggregateStatus.Value
                : AggregateStatus.Archived.Value));
            FollowingRules.Add(new OnlyReviewAuthorCanEditReview(review, readerGuid));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}