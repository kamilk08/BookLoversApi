using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OwnResources.Loggers
{
    internal class OwnResourceSeedQuoteLogger :
        IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration>,
        ISeedProvider<SeedQuote>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Quotes;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnResourceSeedQuoteLogger(
            IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;

            this._logger = logger;

            this._logger = this._logger
                .ForContext("SeedProvider", "[OR_QUOTES_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedQuote>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed quotes.");

            var seedQuotes = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed quotes.");

            return seedQuotes;
        }

        public IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }
    }
}