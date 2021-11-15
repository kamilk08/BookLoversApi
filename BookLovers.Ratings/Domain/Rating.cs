using BookLovers.Base.Domain.Entity;
using BookLovers.Ratings.Domain.RatingStars;

namespace BookLovers.Ratings.Domain
{
    public class Rating : IEntityObject
    {
        internal static int SingleRating = 1;

        public int Id { get; private set; }

        public int BookId { get; private set; }

        public int ReaderId { get; private set; }

        public int Stars { get; private set; }

        private Rating()
        {
        }

        public Rating(int bookId, int readerId, int star)
        {
            this.BookId = bookId;
            this.ReaderId = readerId;
            var numberOfStars = StarList.ChooseStar(star);
            Stars = numberOfStars?.Value ?? -1;
        }

        public void ChangeRating(int star)
        {
            this.Stars = StarList.ChooseStar(star).Value;
        }
    }
}