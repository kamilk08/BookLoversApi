namespace BookLovers.Seed.Models.Configuration
{
    public class OwnResourceConfiguration
    {
        public int UserCount { get; set; }

        public int SeriesCount { get; set; }

        public int TicketsCount { get; set; }

        public int QuotesCount { get; set; }

        public int ReviewsCount { get; set; }

        private OwnResourceConfiguration()
        {
        }

        public static OwnResourceConfiguration Initialize()
        {
            return new OwnResourceConfiguration();
        }

        public OwnResourceConfiguration WithUsers(int count)
        {
            this.UserCount = count;

            return this;
        }

        public OwnResourceConfiguration WithSeries(int count)
        {
            this.SeriesCount = count;

            return this;
        }

        public OwnResourceConfiguration WithTickets(int count)
        {
            this.TicketsCount = count;

            return this;
        }

        public OwnResourceConfiguration WithQuotes(int count)
        {
            this.QuotesCount = count;

            return this;
        }

        public OwnResourceConfiguration WithReviews(int count)
        {
            this.ReviewsCount = count;

            return this;
        }
    }
}