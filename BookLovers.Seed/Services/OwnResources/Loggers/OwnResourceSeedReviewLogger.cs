using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using Serilog;

namespace BookLovers.Seed.Services.OwnResources.Loggers
{
    internal class OwnResourceSeedReviewLogger :
        IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration>,
        ISeedProvider<SeedReview>,
        ISeedProvider
    {
        private readonly IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> _decorated;
        private readonly ILogger _logger;

        public SeedProviderType ProviderType => SeedProviderType.Reviews;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnResourceSeedReviewLogger(
            IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> decorated,
            ILogger logger)
        {
            this._decorated = decorated;
            this._logger = logger;
            this._logger = this._logger
                .ForContext("SeedProvider", "[OR_REVIEWS_SEED_PROVIDER]");
        }

        public async Task<IEnumerable<SeedReview>> ProvideAsync()
        {
            this._logger.Information("{SeedProvider} Seed provider creating seed reviews.");

            var seedReviews = await this._decorated.ProvideAsync();

            this._logger.Information("{SeedProvider} Seed provider created seed reviews.");

            return seedReviews;
        }

        public IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._decorated.SetSeedConfiguration(configuration);

            return this;
        }

        public IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> SetSeedUsers(
            IEnumerable<SeedUser> seedUsers)
        {
            ((OwnResourceSeedReviewProvider) this._decorated).SetSeedUsers(seedUsers);

            return this;
        }
    }
}