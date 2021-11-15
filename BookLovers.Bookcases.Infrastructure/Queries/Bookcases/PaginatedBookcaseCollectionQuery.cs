using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Services;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class PaginatedBookcaseCollectionQuery : IQuery<PaginatedResult<int>>
    {
        public int BookcaseId { get; set; }

        public List<int> ShelvesIds { get; set; }

        public List<int> BookIds { get; set; }

        public bool Descending { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public int SortType { get; set; }

        public string Title { get; set; }

        public PaginatedBookcaseCollectionQuery()
        {
            int page = Page == 0 ? PaginatedResult.DefaultPage : Page;
            int count = Count == 0 ? PaginatedResult.DefaultItemsPerPage : Count;
            int sortType = SortType == 0 ? BookcaseCollectionSortType.ByBookAverage.Value : SortType;
            var bookIds = BookIds ?? new List<int>();
            var shelvesIds = ShelvesIds ?? new List<int>();
            string title = Title ?? string.Empty;

            Page = page;
            Count = count;
            SortType = sortType;
            BookIds = bookIds;
            ShelvesIds = shelvesIds;
            Title = title;
        }

        public PaginatedBookcaseCollectionQuery(
            int bookcaseId,
            List<int> shelvesIds,
            List<int> bookIds,
            int? page,
            int? count,
            bool? descending,
            int? sortType,
            string title)
        {
            BookcaseId = bookcaseId;
            ShelvesIds = shelvesIds;
            BookIds = bookIds;
            Page = page ?? PaginatedResult.DefaultPage;
            Count = count ?? PaginatedResult.DefaultItemsPerPage;
            Descending = descending ?? true;
            SortType = sortType ?? BookcaseCollectionSortType.ByDate.Value;
            Title = title;
        }
    }
}