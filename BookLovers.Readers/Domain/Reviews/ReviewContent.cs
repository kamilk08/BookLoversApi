using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Reviews
{
    public class ReviewContent : ValueObject<ReviewContent>
    {
        public string Review { get; }

        public DateTime ReviewDate { get; }

        public DateTime? EditedDate { get; }

        private ReviewContent()
        {
        }

        public ReviewContent(string review, DateTime reviewDate)
        {
            Review = review;
            ReviewDate = reviewDate;
        }

        public ReviewContent(string review, DateTime editedDate, DateTime reviewDate)
        {
            Review = review;
            EditedDate = new DateTime?(editedDate);
            ReviewDate = reviewDate;
        }

        public static ReviewContent EditReview(
            string review,
            DateTime editReviewDate,
            DateTime reviewDate)
        {
            return new ReviewContent(review, editReviewDate, reviewDate);
        }

        protected override bool EqualsCore(ReviewContent obj)
        {
            return this.Review == obj.Review && this.ReviewDate == obj.ReviewDate
                                             && this.EditedDate == obj.EditedDate;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Review.GetHashCode();
            hash = (hash * 23) + this.ReviewDate.GetHashCode();
            hash = (hash * 23) + this.EditedDate.GetHashCode();

            return hash;
        }
    }
}