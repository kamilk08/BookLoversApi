using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class PaginatedReaderShelvesQuery : IQuery<PaginatedResult<ShelfDto>>
    {
        public int BookcaseId { get; }

        public int Page { get; }

        public int Count { get; }

        public PaginatedReaderShelvesQuery(int bookcaseId, int? page, int? count)
        {
            BookcaseId = bookcaseId;
            Page = page ?? PaginatedResult.DefaultPage;
            Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}