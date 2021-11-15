using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using Serilog;

namespace BookLovers.Seed.Services.OpenLibrary.Loggers
{
    internal class OpenLibraryPublisherSeedLogger : ISeedProvider<SeedPublisher>, ISeedProvider
    {
        private readonly ISeedProvider<SeedPublisher> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Publisher;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryPublisherSeedLogger(ISeedProvider<SeedPublisher> decorated, ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OL_PUBLISHERS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedPublisher>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed publishers.");

            var seedPublishers = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed publishers.");

            return seedPublishers;
        }
    }
}