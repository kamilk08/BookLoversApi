using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services
{
    public class SeedProviderService
    {
        private readonly SeedFactory _factory;

        public SeedProviderService(SeedFactory factory)
        {
            this._factory = factory;
        }

        public async Task<SeedData> CreateSeedDataAsync(SeedDataConfig config)
        {
            var openLibrarySeedData =
                await this._factory.GetSeed<OpenLibrarySeedData, OpenLibrarySeedConfiguration>(
                    config.OpenLibrarySeedConfiguration, SourceType.OpenLibrary);

            var ownResourceSeedData = await this._factory.GetSeed<OwnResourceSeedData, OwnResourceConfiguration>(
                config.OwnResourceConfiguration, SourceType.OwnSource);

            return new SeedData(openLibrarySeedData, ownResourceSeedData);
        }
    }
}