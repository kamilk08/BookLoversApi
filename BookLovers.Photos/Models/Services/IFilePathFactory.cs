using System.Threading.Tasks;

namespace BookLovers.Photos.Models.Services
{
    public interface IFilePathFactory
    {
        ProviderType ProviderType { get; }

        Task<PathResult> GetPath(int id);
    }
}