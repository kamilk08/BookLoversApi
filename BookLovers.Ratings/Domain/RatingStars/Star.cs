using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Ratings.Domain.RatingStars
{
    public class Star : Enumeration
    {
        public static readonly Star Zero = new Star(0, nameof(Zero));
        public static readonly Star One = new Star(1, nameof(One));
        public static readonly Star Two = new Star(2, nameof(Two));
        public static readonly Star Three = new Star(3, nameof(Three));
        public static readonly Star Four = new Star(4, nameof(Four));
        public static readonly Star Five = new Star(5, nameof(Five));

        protected Star()
        {
        }

        protected Star(int value, string name)
            : base(value, name)
        {
        }
    }
}