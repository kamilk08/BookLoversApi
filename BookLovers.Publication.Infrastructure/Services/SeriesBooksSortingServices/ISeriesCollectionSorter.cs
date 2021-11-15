using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Series;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices
{
    public interface ISeriesCollectionSorter
    {
        SeriesCollectionSortingType SortingType { get; }

        Task<PaginatedResult<int>> Sort(PaginatedSeriesCollectionQuery query);
    }
}