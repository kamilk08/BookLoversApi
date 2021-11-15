using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Follows
{
    public class ReaderPaginatedFollowingsQuery : IQuery<PaginatedResult<int>>
    {
        public int ReaderId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public string Value { get; set; }

        public ReaderPaginatedFollowingsQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            this.Page = page;
            this.Count = count;
        }

        public ReaderPaginatedFollowingsQuery(int readerId, string value, int? page, int? count)
        {
            this.ReaderId = readerId;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Value = value;
        }
    }
}