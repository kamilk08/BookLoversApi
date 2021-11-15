using System.Threading.Tasks;
using BookLovers.Photos.Models.Services;

namespace BookLovers.Photos.Models.Books
{
    internal class CoverPathFactory : IFilePathFactory
    {
        private readonly SqlClient _client;

        public ProviderType ProviderType => ProviderType.BookCoverProvider;

        public CoverPathFactory(SqlClient client)
        {
            this._client = client;
        }

        public async Task<PathResult> GetPath(int id)
        {
            var readModel = await _client.QueryAsync<BookCoverReadModel>(Queries.CoverQuery(id));

            return readModel == null
                ? new PathResult(string.Empty, string.Empty)
                : new PathResult(readModel.CoverUrl, readModel.FileName);
        }
    }
}