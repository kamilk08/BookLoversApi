using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Publishers;

namespace BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices
{
    public interface IPublisherCollectionSorter
    {
        PublisherCollectionSortingType SortingType { get; }

        Task<PaginatedResult<int>> Sort(PaginatedPublishersCollectionQuery query);
    }
}