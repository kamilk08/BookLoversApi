using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices.SqlExtensions;

namespace BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices
{
    public class AuthorCollectionAverageSorter : IAuthorCollectionSorter
    {
        private readonly PublicationsContext _context;

        private readonly string _procedureName =
            "SortAuthorBooksByAverage @AUTHOR_ID,@TITLE,@ORDER_BY,@ROW_COUNT,@SKIP";

        private readonly string _totalCountProcedureName = "SortAuthorBooksByAverageTotalCount @AUTHOR_ID,@TITLE";

        public AuthorCollectionSorType SorType => AuthorCollectionSorType.ByAverage;

        public AuthorCollectionAverageSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(AuthorsCollectionQuery query)
        {
            var totalCount = await SqlHelper
                .Initialize(this._context)
                .AddAuthorId(query.AuthorId)
                .AddTitle(query.Title)
                .ExecuteAndGetValueAsync<int>(this._totalCountProcedureName);

            var results = await SqlHelper.Initialize(this._context)
                .AddAuthorId(query.AuthorId)
                .AddTitle(query.Title)
                .AddSorting(query.Descending)
                .AddRowCount(query.Count)
                .AddOffSet(query.Page, query.Count)
                .ExecuteAndGetValuesAsync<AuthorCollectionRecord>(this._procedureName);

            var res = results.Select(s => s.BookId).ToList();

            return new PaginatedResult<int>(res, query.Page, query.Count, totalCount);
        }
    }
}