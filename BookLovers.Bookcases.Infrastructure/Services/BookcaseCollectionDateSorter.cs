using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Services.SqlExtensions;

namespace BookLovers.Bookcases.Infrastructure.Services
{
    internal class BookcaseCollectionDateSorter : IBookcaseCollectionSorter
    {
        private readonly BookcaseContext _context;

        private readonly string _sortByDateProcedureName =
            "SortBookcaseCollectionByDate @BOOKCASE_ID, @SHELVES_IDS, @BOOK_IDS,@TITLE, @ORDER_BY,@ROW_COUNT,@SKIP";

        private readonly string _sortByDateTotalCountProcedureName =
            "SortBookcaseCollectionByDateTotalCount @BOOKCASE_ID,@SHELVES_IDS,@BOOK_IDS,@TITLE";

        public BookcaseCollectionSortType SortType => BookcaseCollectionSortType.ByDate;

        public BookcaseCollectionDateSorter(BookcaseContext context) => _context = context;

        public async Task<PaginatedResult<int>> Sort(
            PaginatedBookcaseCollectionQuery query)
        {
            int totalCount = await SqlHelper
                .Initialize(_context)
                .AddBookcaseIdParameter(query.BookcaseId)
                .AddShelvesIdsParameter(query.ShelvesIds)
                .AddBookIdsParameter(query.BookIds)
                .AddTitle(query.Title)
                .ExecuteAndGetValueAsync<int>(_sortByDateTotalCountProcedureName);

            var queryResult = await SqlHelper.Initialize(_context)
                .AddBookcaseIdParameter(query.BookcaseId)
                .AddShelvesIdsParameter(query.ShelvesIds)
                .AddBookIdsParameter(query.BookIds)
                .AddTitle(query.Title)
                .AddSorting(query.Descending)
                .AddRowCount(query.Count)
                .AddOffSet(query.Page, query.Count)
                .ExecuteAndGetValuesAsync<BookcaseCollectionRecord>(_sortByDateProcedureName);

            var results = queryResult.Select(s => s.BookId).Distinct().ToList();

            return new PaginatedResult<int>(results, query.Page, query.Count, totalCount);
        }
    }
}