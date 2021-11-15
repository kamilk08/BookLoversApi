using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookRating : ValueObject<BookRating>
    {
        public double AverageRating { get; }

        public int RatingCounts { get; }

        private BookRating()
        {
            this.AverageRating = 0.0;
            this.RatingCounts = 0;
        }

        internal BookRating(double averageRating, int ratingCount)
        {
            this.AverageRating = averageRating;
            this.RatingCounts = ratingCount;
        }

        public static BookRating DefaultRating()
        {
            return new BookRating();
        }

        public BookRating ChangeRating(double averageRating, int ratingCount)
        {
            return new BookRating(averageRating, ratingCount);
        }

        protected override bool EqualsCore(BookRating obj)
        {
            return Math.Abs(this.AverageRating - obj.AverageRating) < 0.001
                   && this.RatingCounts == obj.RatingCounts;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.RatingCounts.GetHashCode();
            hash = (hash * 23) + this.AverageRating.GetHashCode();

            return hash;
        }
    }
}