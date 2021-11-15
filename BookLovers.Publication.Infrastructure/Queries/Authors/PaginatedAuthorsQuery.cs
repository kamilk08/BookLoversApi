using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class PaginatedAuthorsQuery : IQuery<PaginatedResult<AuthorDto>>
    {
        public string Value { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public PaginatedAuthorsQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public PaginatedAuthorsQuery(string value, int page, int count)
        {
            this.Value = value;
            this.Page = page == 0 ? PaginatedResult.DefaultPage : this.Page;
            this.Count = count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
        }
    }
}