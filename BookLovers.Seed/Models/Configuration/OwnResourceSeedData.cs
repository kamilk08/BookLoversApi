using System.Collections.Generic;

namespace BookLovers.Seed.Models.Configuration
{
    public class OwnResourceSeedData
    {
        public IEnumerable<SeedUser> Users { get; }

        public IEnumerable<SeedSeries> Series { get; }

        public IEnumerable<SeedTicket> Tickets { get; }

        public IEnumerable<SeedQuote> Quotes { get; }

        public IEnumerable<SeedReview> Reviews { get; }

        public OwnResourceSeedData(
            IEnumerable<SeedUser> users,
            IEnumerable<SeedSeries> series,
            IEnumerable<SeedTicket> tickets,
            IEnumerable<SeedQuote> quotes,
            IEnumerable<SeedReview> reviews)
        {
            this.Users = users;
            this.Series = series;
            this.Tickets = tickets;
            this.Quotes = quotes;
            this.Reviews = reviews;
        }
    }
}