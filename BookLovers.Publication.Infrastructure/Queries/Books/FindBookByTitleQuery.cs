using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class FindBookByTitleQuery : IQuery<PaginatedResult<BookDto>>
    {
        public string Value { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public FindBookByTitleQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public FindBookByTitleQuery(string value, int? page, int? count)
        {
            this.Value = value;

            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}