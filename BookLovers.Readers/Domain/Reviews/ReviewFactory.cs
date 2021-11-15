using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Reviews.BusinessRules;
using BookLovers.Readers.Domain.Reviews.Services;

namespace BookLovers.Readers.Domain.Reviews
{
    public class ReviewFactory
    {
        private readonly List<Func<Review, IBusinessRule>> _rules =
            new List<Func<Review, IBusinessRule>>();

        public ReviewFactory()
        {
            _rules.Add(review => new AggregateMustBeActive(review.AggregateStatus.Value));
            _rules.Add(review => new ReviewMustHaveAnOwner(review.ReviewIdentification));
            _rules.Add(review => new ReviewMustBeAssociatedWithBook(review.ReviewIdentification));
        }

        public Review Create(ReviewParts reviewParts)
        {
            var identification = new ReviewIdentification(reviewParts.BookGuid, reviewParts.ReaderGuid);
            var reviewContent = new ReviewContent(reviewParts.Content, reviewParts.ReviewDate);
            var reviewSpoiler = new ReviewSpoiler(reviewParts.MarkedAsSpoiler, false);

            var review = new Review(reviewParts.ReviewGuid, identification, reviewContent, reviewSpoiler);

            foreach (var rule in _rules)
            {
                if (!rule(review).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(review).BrokenRuleMessage);
            }

            return review;
        }
    }
}