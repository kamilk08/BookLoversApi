using System.Threading.Tasks;
using BookLovers.Seed.Models;

namespace BookLovers.Seed.Services
{
    public interface ISeedFactory
    {
        SourceType SourceType { get; }
    }

    public interface ISeedFactory<TSeed, TConfiguration> : ISeedFactory
    {
        Task<TSeed> CreateSeedAsync(TConfiguration seedConfiguration);
    }
}