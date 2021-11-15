using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Authors;

namespace BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices
{
    public interface IAuthorCollectionSorter
    {
        AuthorCollectionSorType SorType { get; }

        Task<PaginatedResult<int>> Sort(AuthorsCollectionQuery query);
    }
}