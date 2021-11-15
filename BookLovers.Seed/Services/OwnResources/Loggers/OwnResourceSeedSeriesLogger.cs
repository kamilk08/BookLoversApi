using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OwnResources.Loggers
{
    internal class OwnResourceSeedSeriesLogger :
        IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration>,
        ISeedProvider<SeedSeries>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Series;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnResourceSeedSeriesLogger(
            IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;

            this._logger = this._logger
                .ForContext("SeedProvider", "[OR_SERIES_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedSeries>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed series.");

            var seedSeries = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed series.");

            return seedSeries;
        }

        public IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }
    }
}