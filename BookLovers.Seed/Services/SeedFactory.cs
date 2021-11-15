using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;

namespace BookLovers.Seed.Services
{
    public class SeedFactory
    {
        private readonly Dictionary<SourceType, ISeedFactory> _factories;

        public SeedFactory(Dictionary<SourceType, ISeedFactory> factories) => this._factories = factories;

        public Task<TSeed> GetSeed<TSeed, TConfiguration>(
            TConfiguration seedConfiguration,
            SourceType sourceType)
        {
            return this.GetFactory<TSeed, TConfiguration>(sourceType).CreateSeedAsync(seedConfiguration);
        }

        private ISeedFactory<TSeed, TConfiguration> GetFactory<TSeed, TConfiguration>(
            SourceType sourceType)
        {
            return this._factories[sourceType] as ISeedFactory<TSeed, TConfiguration>;
        }
    }
}