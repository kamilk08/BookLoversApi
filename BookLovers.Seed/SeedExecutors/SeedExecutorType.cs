using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Seed.SeedExecutors
{
    public class SeedExecutorType : Enumeration
    {
        public static readonly SeedExecutorType PublisherSeedExecutor = new SeedExecutorType(1, nameof(PublisherSeedExecutor));
        public static readonly SeedExecutorType AuthorsSeedExecutor = new SeedExecutorType(2, nameof(AuthorsSeedExecutor));
        public static readonly SeedExecutorType BooksSeedExecutor = new SeedExecutorType(3, "BookSeedExecutor");
        public static readonly SeedExecutorType SeriesSeedExecutor = new SeedExecutorType(4, nameof(SeriesSeedExecutor));
        public static readonly SeedExecutorType UsersSeedExecutor = new SeedExecutorType(5, nameof(UsersSeedExecutor));
        public static readonly SeedExecutorType FollowersExecutor = new SeedExecutorType(6, "UserFollowersExecutor");
        public static readonly SeedExecutorType UserTicketsExecutor = new SeedExecutorType(7, nameof(UserTicketsExecutor));
        public static readonly SeedExecutorType QuotesSeedExecutor = new SeedExecutorType(8, nameof(QuotesSeedExecutor));
        public static readonly SeedExecutorType ReviewsSeedExecutor = new SeedExecutorType(9, nameof(ReviewsSeedExecutor));
        public static readonly SeedExecutorType BookcaseSeedExecutor = new SeedExecutorType(10, nameof(BookcaseSeedExecutor));
        public static readonly SeedExecutorType RatingsSeedExecutor = new SeedExecutorType(11, nameof(RatingsSeedExecutor));

        protected SeedExecutorType(int value, string name)
            : base(value, name)
        {
        }
    }
}