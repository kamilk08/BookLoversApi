using System.Threading.Tasks;

namespace BookLovers.Auth.Application.Contracts
{
    public interface ISeedService
    {
        Task SeedAsync();
    }
}