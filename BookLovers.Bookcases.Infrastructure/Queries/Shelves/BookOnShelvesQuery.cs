using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class BookOnShelvesQuery : IQuery<PaginatedResult<KeyValuePair<string, int>>>
    {
        public int BookId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public BookOnShelvesQuery()
        {
            int page = Page == 0 ? PaginatedResult.DefaultPage : Page;
            int count = Count == 0 ? PaginatedResult.DefaultItemsPerPage : Count;
            Page = page;
            Count = count;
        }

        public BookOnShelvesQuery(int bookId, int? page, int? count)
        {
            BookId = bookId;
            Page = page ?? PaginatedResult.DefaultPage;
            Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}