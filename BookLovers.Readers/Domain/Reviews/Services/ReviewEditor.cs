using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Readers.Domain.Reviews.Services
{
    public class ReviewEditor : IDomainService<Review>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ReviewEditor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void EditReview(Review review, ReviewParts reviewParts)
        {
            var reviewContent =
                new ReviewContent(reviewParts.Content, reviewParts.EditDate, review.ReviewContent.ReviewDate);

            if (review == null)
                throw new BusinessRuleNotMetException($"Review does not exist.");

            review.EditReview(_contextAccessor.UserGuid, reviewContent, reviewParts.MarkedAsSpoiler);

            if (reviewParts.MarkedAsSpoiler == review.ReviewSpoiler.MarkedAsSpoilerByReader)
                return;

            review.AddOrRemoveSpoilerMark();
        }
    }
}