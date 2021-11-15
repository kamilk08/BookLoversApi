using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Authors
{
    internal class AuthorsCollectionByIdHandler :
        IQueryHandler<AuthorsCollectionQuery, PaginatedResult<int>>
    {
        private readonly IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter> _sorters;

        public AuthorsCollectionByIdHandler(
            IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter> sorters)
        {
            this._sorters = sorters;
        }

        public Task<PaginatedResult<int>> HandleAsync(AuthorsCollectionQuery query)
        {
            var collectionSorter = this._sorters.Values.SingleOrDefault(p => p.SorType.Value == query.SortType);

            return collectionSorter == null
                ? Task.FromResult(new PaginatedResult<int>(query.Page))
                : collectionSorter.Sort(query);
        }
    }
}