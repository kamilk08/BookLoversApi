using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Reviews
{
    public class ReaderReviewsIdsQuery : IQuery<PaginatedResult<int>>
    {
        public int ReaderId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public int SortType { get; set; }

        public bool Descending { get; set; }

        public ReaderReviewsIdsQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            var sortType = this.SortType == 0 ? ReviewsSortingType.ByDate.Value : this.SortType;

            this.Page = page;
            this.Count = count;
            this.SortType = sortType;
        }

        public ReaderReviewsIdsQuery(
            int readerId,
            int? page,
            int? count,
            int? sortType,
            bool descending)
        {
            this.ReaderId = readerId;
            this.Page = page ?? PaginatedResult.DefaultPage;

            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.SortType = sortType ?? ReviewsSortingType.ByDate.Value;
            this.Descending = descending;
        }
    }
}