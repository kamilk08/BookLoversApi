using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OwnResources.Loggers
{
    internal class OwnResourceSeedUserLogger :
        IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration>,
        ISeedProvider<SeedUser>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Users;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnResourceSeedUserLogger(
            IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OR_USERS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedUser>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed users.");

            var seedUsers = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed users.");

            return seedUsers;
        }

        public IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }
    }
}