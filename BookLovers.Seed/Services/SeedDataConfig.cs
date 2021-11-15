using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services
{
    public class SeedDataConfig
    {
        public OpenLibrarySeedConfiguration OpenLibrarySeedConfiguration { get; }

        public OwnResourceConfiguration OwnResourceConfiguration { get; }

        public SeedDataConfig(
            OpenLibrarySeedConfiguration openLibrarySeedConfiguration,
            OwnResourceConfiguration ownResourceConfiguration)
        {
            this.OpenLibrarySeedConfiguration = openLibrarySeedConfiguration;
            this.OwnResourceConfiguration = ownResourceConfiguration;
        }
    }
}