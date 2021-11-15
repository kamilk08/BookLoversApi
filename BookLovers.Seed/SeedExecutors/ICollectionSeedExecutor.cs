using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLovers.Seed.SeedExecutors
{
    public interface ICollectionSeedExecutor<T> : ISeedExecutor
    {
        Task SeedAsync(IEnumerable<T> seed);
    }
}