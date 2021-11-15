using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Queries.Librarians
{
    public class LibrarianPageQuery : IQuery<PaginatedResult<LibrarianDto>>
    {
        public List<int> Ids { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public LibrarianPageQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            var ids = this.Ids ?? new List<int>();

            this.Page = page;
            this.Count = count;
            this.Ids = ids;
        }

        public LibrarianPageQuery(List<int> ids, int? page, int? count)
        {
            this.Ids = ids;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}