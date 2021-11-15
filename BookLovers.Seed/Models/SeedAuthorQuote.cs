using System;

namespace BookLovers.Seed.Models
{
    public class SeedAuthorQuote
    {
        public Guid AuthorGuid { get; }

        public string Quote { get; }

        public SeedAuthorQuote(Guid authorGuid, string quote)
        {
            this.AuthorGuid = authorGuid;
            this.Quote = quote;
        }
    }
}