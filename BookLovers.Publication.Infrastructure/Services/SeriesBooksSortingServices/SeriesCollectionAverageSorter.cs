using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Series;
using BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices.SqlExtensions;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices
{
    internal class SeriesCollectionAverageSorter : ISeriesCollectionSorter
    {
        private readonly PublicationsContext _context;
        private readonly string procedureName = "SortSeriesBooksByAverage @SERIES_ID,@TITLE,@ORDER_BY,@ROW_COUNT,@SKIP";
        private readonly string totalCountProcedureName = "SortSeriesBooksByAverageTotalCount @SERIES_ID ,@TITLE";

        public SeriesCollectionSortingType SortingType => SeriesCollectionSortingType.ByAverage;

        public SeriesCollectionAverageSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(
            PaginatedSeriesCollectionQuery query)
        {
            var totalCount = await SqlHelper
                .Initialize(this._context)
                .AddSeriesId(query.SeriesId)
                .AddTitle(query.Title)
                .ExecuteAndGetValueAsync<int>(this.totalCountProcedureName);

            var results = await SqlHelper.Initialize(this._context)
                .AddSeriesId(query.SeriesId)
                .AddTitle(query.Title)
                .AddSorting(query.Descending)
                .AddOffSet(query.Page, query.Count)
                .AddRowCount(query.Count)
                .ExecuteAndGetValuesAsync<SeriesBookRecord>(this.procedureName);

            var res = results.Select(s => s.BookId).ToList();

            return new PaginatedResult<int>(res, query.Page, query.Count, totalCount);
        }
    }
}