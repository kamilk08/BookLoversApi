using System.Threading.Tasks;
using BookLovers.Photos.Models.Services;

namespace BookLovers.Photos.Models.Avatars
{
    internal class AvatarPathFactory : IFilePathFactory
    {
        private readonly SqlClient _client;

        public ProviderType ProviderType => ProviderType.AvatarImageProvider;

        public AvatarPathFactory(SqlClient client)
        {
            this._client = client;
        }

        public async Task<PathResult> GetPath(int id)
        {
            var path = await _client.QueryAsync<AvatarReadModel>(Queries.AvatarQuery(id));

            return path == null
                ? new PathResult(string.Empty, string.Empty)
                : new PathResult(path.AvatarUrl, path.MimeType);
        }
    }
}