using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Seed.Models;

namespace BookLovers.Seed.Services
{
    public interface ISeedProvider
    {
        SeedProviderType ProviderType { get; }

        SourceType SourceType { get; }
    }

    public interface ISeedProvider<T> : ISeedProvider
        where T : class
    {
        Task<IEnumerable<T>> ProvideAsync();
    }
}