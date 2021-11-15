using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using BookLovers.Seed.Services.OwnResources.Loggers;

namespace BookLovers.Seed.Services.OwnResources
{
    public class OwnSeedResourceFactory :
        ISeedFactory<OwnResourceSeedData, OwnResourceConfiguration>,
        ISeedFactory
    {
        private readonly List<ISeedProvider> _seedProviders;

        public SourceType SourceType => SourceType.OwnSource;

        public OwnSeedResourceFactory(List<ISeedProvider> seedProviders) => this._seedProviders =
            seedProviders.Where(p => p.SourceType.Equals(SourceType.OwnSource)).ToList();

        public async Task<OwnResourceSeedData> CreateSeedAsync(
            OwnResourceConfiguration seedConfiguration)
        {
            var seedUsers = await this
                .GetConfigurableProvider<SeedUser, OwnResourceConfiguration>(SeedProviderType.Users)
                .SetSeedConfiguration(seedConfiguration).ProvideAsync();

            var seedSeries = await this
                .GetConfigurableProvider<SeedSeries, OwnResourceConfiguration>(SeedProviderType.Series)
                .SetSeedConfiguration(seedConfiguration).ProvideAsync();

            var seedTickets = await this
                .GetConfigurableProvider<SeedTicket, OwnResourceConfiguration>(SeedProviderType.UserTickets)
                .SetSeedConfiguration(seedConfiguration).ProvideAsync();

            var seedQuotes = await this
                .GetConfigurableProvider<SeedQuote, OwnResourceConfiguration>(SeedProviderType.Quotes)
                .SetSeedConfiguration(seedConfiguration).ProvideAsync();

            var seedReviews =
                await ((OwnResourceSeedReviewLogger) this.GetConfigurableProvider<SeedReview, OwnResourceConfiguration>(
                        SeedProviderType.Reviews))
                    .SetSeedUsers(seedUsers)
                    .SetSeedConfiguration(seedConfiguration)
                    .ProvideAsync();

            return new OwnResourceSeedData(seedUsers, seedSeries, seedTickets, seedQuotes, seedReviews);
        }

        private ISeedProvider<T> GetProvider<T>(SeedProviderType providerType)
            where T : class
        {
            return this._seedProviders.Single(p => p.ProviderType == providerType) as
                ISeedProvider<T>;
        }

        private IConfigurableSeedProvider<TSeed, TConfiguration> GetConfigurableProvider<TSeed, TConfiguration>(
            SeedProviderType providerType)
            where TSeed : class
        {
            return this._seedProviders.Single(p => p.ProviderType == providerType) as
                IConfigurableSeedProvider<TSeed, TConfiguration>;
        }
    }
}