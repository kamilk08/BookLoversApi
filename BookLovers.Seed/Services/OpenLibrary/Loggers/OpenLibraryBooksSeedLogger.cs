using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OpenLibrary.Loggers
{
    internal class OpenLibraryBooksSeedLogger :
        IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration>,
        ISeedProvider<SeedBook>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Books;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryBooksSeedLogger(
            IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OL_BOOKS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedBook>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed books.");

            var seedBooks = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed books.");

            return seedBooks;
        }

        public IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration> SetSeedConfiguration(
            OpenLibrarySeedConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }
    }
}