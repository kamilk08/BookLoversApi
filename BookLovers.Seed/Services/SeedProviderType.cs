using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Seed.Services
{
    public class SeedProviderType : Enumeration
    {
        public static readonly SeedProviderType Publisher = new SeedProviderType(1, nameof(Publisher));
        public static readonly SeedProviderType Authors = new SeedProviderType(2, nameof(Authors));
        public static readonly SeedProviderType Books = new SeedProviderType(3, nameof(Books));
        public static readonly SeedProviderType Series = new SeedProviderType(4, nameof(Series));
        public static readonly SeedProviderType Users = new SeedProviderType(5, nameof(Users));
        public static readonly SeedProviderType UserTickets = new SeedProviderType(6, "User tickets");
        public static readonly SeedProviderType Quotes = new SeedProviderType(7, nameof(Quotes));
        public static readonly SeedProviderType Reviews = new SeedProviderType(8, nameof(Reviews));

        protected SeedProviderType(int value, string name)
            : base(value, name)
        {
        }
    }
}