using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OpenLibrary
{
    internal class OpenLibrarySeedFactory :
        ISeedFactory<OpenLibrarySeedData, OpenLibrarySeedConfiguration>,
        ISeedFactory
    {
        private readonly List<ISeedProvider> _seedProviders;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibrarySeedFactory(List<ISeedProvider> seedProviders)
        {
            _seedProviders = seedProviders.Where(p => p.SourceType.Equals(SourceType.OpenLibrary)).ToList();
        }

        public async Task<OpenLibrarySeedData> CreateSeedAsync(
            OpenLibrarySeedConfiguration openLibrarySeedConfiguration)
        {
            var externalBooks =
                await ((IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration>) this.GetSeedProvider(
                        SeedProviderType.Books))
                    .SetSeedConfiguration(openLibrarySeedConfiguration).ProvideAsync();

            var externalAuthors =
                await ((ISeedProvider<SeedAuthor>) this.GetSeedProvider(SeedProviderType.Authors)).ProvideAsync();

            var openLibrarySeedData = new OpenLibrarySeedData(externalAuthors, externalBooks,
                await ((ISeedProvider<SeedPublisher>) this.GetSeedProvider(SeedProviderType.Publisher)).ProvideAsync());

            return openLibrarySeedData;
        }

        private ISeedProvider GetSeedProvider(SeedProviderType providerType) =>
            this._seedProviders.Single(p => p.ProviderType == providerType);
    }
}