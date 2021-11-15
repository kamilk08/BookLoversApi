using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OwnResources
{
    internal class OwnResourceSeedQuoteProvider :
        IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration>,
        ISeedProvider<SeedQuote>,
        ISeedProvider
    {
        private OwnResourceConfiguration _configuration;

        public SeedProviderType ProviderType => SeedProviderType.Quotes;

        public SourceType SourceType => SourceType.OwnSource;

        public Task<IEnumerable<SeedQuote>> ProvideAsync()
        {
            var source = new List<SeedQuote>();
            var num = (this._configuration.QuotesCount + 1) / 2;

            for (var index = 1; index < num; ++index)
            {
                var seedQuote = new SeedQuote();

                seedQuote.AddedAt = DateTime.UtcNow.AddMinutes(index);
                seedQuote.Quote = Guid.NewGuid().ToString("N");

                source.Add(seedQuote);
            }

            for (var index = num; index < this._configuration.QuotesCount + 1; ++index)
            {
                var seedQuote = new SeedQuote();

                seedQuote.AddedAt = DateTime.UtcNow.AddMinutes(index);
                seedQuote.Quote = Guid.NewGuid().ToString("N");

                source.Add(seedQuote);
            }

            return Task.FromResult(source.AsEnumerable());
        }

        public IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._configuration = configuration;

            return this;
        }
    }
}