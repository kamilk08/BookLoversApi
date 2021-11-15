using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OwnResources.Loggers
{
    internal class OwnResourceSeedTicketsLogger :
        IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration>,
        ISeedProvider<SeedTicket>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.UserTickets;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnResourceSeedTicketsLogger(
            IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OR_TICKETS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedTicket>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed tickets.");

            var seedTickets = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed tickets.");

            return seedTickets;
        }

        public IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }
    }
}