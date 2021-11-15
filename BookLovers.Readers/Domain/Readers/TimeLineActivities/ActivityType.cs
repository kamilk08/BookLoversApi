using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.Readers.TimeLineActivities
{
    public class ActivityType : Enumeration
    {
        public static readonly ActivityType NewBookInBookCase = new ActivityType(1, "New book in bookcase");
        public static readonly ActivityType NewFollower = new ActivityType(2, "New follower");
        public static readonly ActivityType LostFollower = new ActivityType(3, "Lost follower");
        public static readonly ActivityType NewReview = new ActivityType(4, "New review");
        public static readonly ActivityType EditReview = new ActivityType(5, "Edit review");
        public static readonly ActivityType ReviewRemoved = new ActivityType(6, "Review removed");
        public static readonly ActivityType AddedBook = new ActivityType(7, "Added book");
        public static readonly ActivityType AddedAuthor = new ActivityType(8, "Added author");
        public static readonly ActivityType FavouriteAuthor = new ActivityType(9, "Favourite author");
        public static readonly ActivityType FavouriteBook = new ActivityType(10, "Favourite book");
        public static readonly ActivityType AddedPublisher = new ActivityType(11, "Added publisher");
        public static readonly ActivityType AddedSeries = new ActivityType(12, "Added series");
        public static readonly ActivityType NewAuthorQuote = new ActivityType(15, "New author quote");
        public static readonly ActivityType NewBookQuote = new ActivityType(16, "New book quoute");

        public ActivityType()
        {
        }

        [JsonConstructor]
        protected ActivityType(byte value, string name)
            : base(value, name)
        {
        }
    }
}