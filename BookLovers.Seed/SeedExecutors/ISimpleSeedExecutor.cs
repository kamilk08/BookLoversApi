using System.Threading.Tasks;

namespace BookLovers.Seed.SeedExecutors
{
    public interface ISimpleSeedExecutor<T> : ISeedExecutor
    {
        Task SeedAsync(T seed);
    }
}