using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Services;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Bookcases
{
    internal class PaginatedBookcaseCollectionHandler :
        IQueryHandler<PaginatedBookcaseCollectionQuery, PaginatedResult<int>>
    {
        private readonly IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter> _sorters;

        public PaginatedBookcaseCollectionHandler(
            IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter> sorters)
        {
            _sorters = sorters;
        }

        public Task<PaginatedResult<int>> HandleAsync(
            PaginatedBookcaseCollectionQuery query)
        {
            var sorter = _sorters.Values
                .SingleOrDefault(p => p.SortType.Value == query.SortType);

            return sorter == null ? Task.FromResult(new PaginatedResult<int>(query.Page)) : sorter.Sort(query);
        }
    }
}