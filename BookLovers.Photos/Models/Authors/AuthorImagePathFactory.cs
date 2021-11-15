using System.Threading.Tasks;
using BookLovers.Photos.Models.Services;

namespace BookLovers.Photos.Models.Authors
{
    internal class AuthorImagePathFactory : IFilePathFactory
    {
        private readonly SqlClient _client;

        public ProviderType ProviderType => ProviderType.AuthorImageProvider;

        public AuthorImagePathFactory(SqlClient client)
        {
            this._client = client;
        }

        public async Task<PathResult> GetPath(int id)
        {
            var path = await _client.QueryAsync<AuthorImageReadModel>(Queries.AuthorImageQuery(id));

            return path == null
                ? new PathResult(string.Empty, string.Empty)
                : new PathResult(path.AuthorPictureUrl, path.MimeType);
        }
    }
}