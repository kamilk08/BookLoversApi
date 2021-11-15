using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Models
{
    public class SeedData
    {
        public OpenLibrarySeedData OpenLibrarySeedData { get; }

        public OwnResourceSeedData OwnResourceSeedData { get; }

        public SeedData(
            OpenLibrarySeedData openLibrarySeedData,
            OwnResourceSeedData ownResourceSeedData)
        {
            this.OpenLibrarySeedData = openLibrarySeedData;
            this.OwnResourceSeedData = ownResourceSeedData;
        }
    }
}