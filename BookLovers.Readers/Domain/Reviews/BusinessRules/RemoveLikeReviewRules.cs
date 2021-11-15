using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Likes;

namespace BookLovers.Readers.Domain.Reviews.BusinessRules
{
    internal class RemoveLikeReviewRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public RemoveLikeReviewRules(Review review, Like likes)
        {
            FollowingRules.Add(new AggregateMustExist(review.Guid));
            FollowingRules.Add(new AggregateMustBeActive(review.AggregateStatus != null
                ? review.AggregateStatus.Value
                : AggregateStatus.Archived.Value));
            FollowingRules.Add(new ReviewMustContainSelectedLike(review, likes));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}