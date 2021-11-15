using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OwnResources
{
    internal class OwnResourceSeedSeriesProvider :
        IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration>,
        ISeedProvider<SeedSeries>,
        ISeedProvider
    {
        private OwnResourceConfiguration _configuration;

        public SeedProviderType ProviderType => SeedProviderType.Series;

        public SourceType SourceType => SourceType.OwnSource;

        public Task<IEnumerable<SeedSeries>> ProvideAsync()
        {
            var source = new List<SeedSeries>();

            for (var index = 1; index < this._configuration.SeriesCount + 1; ++index)
                source.Add(new SeedSeries(Guid.NewGuid(), $"Series-{index}"));

            return Task.FromResult(source.AsEnumerable());
        }

        public IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._configuration = configuration;

            return this;
        }
    }
}