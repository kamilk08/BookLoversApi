using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Series;
using BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class PaginatedSeriesCollectionHandler :
        IQueryHandler<PaginatedSeriesCollectionQuery, PaginatedResult<int>>
    {
        private readonly IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter> _sorters;

        public PaginatedSeriesCollectionHandler(
            IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter> sorters)
        {
            this._sorters = sorters;
        }

        public Task<PaginatedResult<int>> HandleAsync(
            PaginatedSeriesCollectionQuery query)
        {
            var collectionSorter = this._sorters.Values
                .SingleOrDefault(p => p.SortingType.Value == query.SortType);

            return collectionSorter == null
                ? Task.FromResult(new PaginatedResult<int>(query.Page))
                : collectionSorter.Sort(query);
        }
    }
}