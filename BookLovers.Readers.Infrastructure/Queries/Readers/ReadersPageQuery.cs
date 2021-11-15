using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReadersPageQuery : IQuery<PaginatedResult<ReaderDto>>
    {
        public string Value { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public ReadersPageQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public ReadersPageQuery(string value, int? page, int? count)
        {
            this.Value = value;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}