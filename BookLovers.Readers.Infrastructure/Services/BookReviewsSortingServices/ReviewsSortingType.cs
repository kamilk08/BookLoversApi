using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices
{
    public class ReviewsSortingType : Enumeration
    {
        public static readonly ReviewsSortingType ByDate = new ReviewsSortingType(1, "By Date");
        public static readonly ReviewsSortingType ByLikes = new ReviewsSortingType(2, "By Likes");

        protected ReviewsSortingType(int value, string name)
            : base(value, name)
        {
        }
    }
}