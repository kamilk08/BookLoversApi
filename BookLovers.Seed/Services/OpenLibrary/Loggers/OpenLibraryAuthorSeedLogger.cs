using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using Serilog;

namespace BookLovers.Seed.Services.OpenLibrary.Loggers
{
    internal class OpenLibraryAuthorSeedLogger : ISeedProvider<SeedAuthor>, ISeedProvider
    {
        private readonly ISeedProvider<SeedAuthor> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Authors;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryAuthorSeedLogger(ISeedProvider<SeedAuthor> decorated, ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OL_AUTHORS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedAuthor>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed authors.");

            var seedAuthors = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed authors.");

            return seedAuthors;
        }
    }
}