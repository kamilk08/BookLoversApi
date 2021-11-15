using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices
{
    public class AuthorCollectionTitleSorter : IAuthorCollectionSorter
    {
        private readonly PublicationsContext _context;

        public AuthorCollectionSorType SorType => AuthorCollectionSorType.ByTitle;

        public AuthorCollectionTitleSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(AuthorsCollectionQuery query)
        {
            var booksStartingWith = this._context.Books
                .Include(p => p.Authors)
                .Where(p => p.Authors.Any(a => a.Id == query.AuthorId))
                .FindAuthorBooksStartingWith(query.AuthorId, query.Title);

            var totalCountQuery = booksStartingWith.DeferredCount();

            IOrderedQueryable<BookReadModel> orderedQuery;

            if (!query.Descending)
                orderedQuery = booksStartingWith.OrderByDescending(p => p.Title);
            else
                orderedQuery = booksStartingWith.OrderBy(p => p.Title);

            var resultsQuery = orderedQuery.Paginate(query.Page, query.Count)
                .Select(s => s.Id).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var paginatedResult = results == null
                ? new PaginatedResult<int>(query.Page)
                : new PaginatedResult<int>(results, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}