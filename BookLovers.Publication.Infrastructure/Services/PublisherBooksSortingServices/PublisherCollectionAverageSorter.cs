using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Queries.SqlExtensionsParameter;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices.SqlExtensions;

namespace BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices
{
    internal class PublisherCollectionAverageSorter : IPublisherCollectionSorter
    {
        private readonly PublicationsContext _context;

        private readonly string _procedureName =
            "SortPublisherBooksByAverage @PUBLISHER_ID,@TITLE,@ORDER_BY,@ROW_COUNT,@SKIP";

        private readonly string _totalCountProcedureName = "SortPublisherBooksByAverageTotalCount @PUBLISHER_ID,@TITLE";

        public PublisherCollectionSortingType SortingType => PublisherCollectionSortingType.ByBookAverage;

        public PublisherCollectionAverageSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(
            PaginatedPublishersCollectionQuery query)
        {
            var results = await SqlHelper.Initialize(this._context)
                .AddPublisherId(query.PublisherId)
                .AddTitle(query.Title)
                .AddSorting(query.Descending)
                .AddRowCount(query.Count)
                .AddOffSet(query.Count, query.Page)
                .ExecuteAndGetValuesAsync<PublisherBookRecord>(this._procedureName);

            var totalCount = await SqlHelper
                .Initialize(this._context).AddPublisherId(query.PublisherId)
                .AddTitle(query.Title)
                .ExecuteAndGetValueAsync<int>(this._totalCountProcedureName);

            var paginatedResult = new PaginatedResult<int>(
                results.Select(s => s.BookId).ToList(), query.Page, query.Count,
                totalCount);

            return paginatedResult;
        }
    }
}