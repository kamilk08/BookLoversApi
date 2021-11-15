using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Publishers
{
    internal class PaginatedPublisherCollectionsHandler :
        IQueryHandler<PaginatedPublishersCollectionQuery, PaginatedResult<int>>
    {
        private readonly IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter> _sorters;

        public PaginatedPublisherCollectionsHandler(
            IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter> sorters)
        {
            this._sorters = sorters;
        }

        public Task<PaginatedResult<int>> HandleAsync(
            PaginatedPublishersCollectionQuery query)
        {
            var collectionSorter = this._sorters.Values
                .SingleOrDefault(p => p.SortingType.Value == query.SortType);

            return collectionSorter != null
                ? collectionSorter.Sort(query)
                : Task.FromResult(new PaginatedResult<int>(query.Page));
        }
    }
}