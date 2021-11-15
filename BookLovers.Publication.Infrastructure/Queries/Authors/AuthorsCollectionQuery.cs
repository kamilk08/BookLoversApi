using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorsCollectionQuery : IQuery<PaginatedResult<int>>
    {
        public int AuthorId { get; set; }

        public string Title { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public bool Descending { get; set; }

        public int SortType { get; set; }

        public AuthorsCollectionQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            int sortType = this.SortType == 0 ? AuthorCollectionSorType.ByTitle.Value : this.SortType;

            this.Page = page;
            this.Count = count;
            this.SortType = sortType;
        }

        public AuthorsCollectionQuery(
            int authorId,
            string title,
            int? page,
            int? count,
            bool? descending,
            int? sortType)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Page = page ?? PaginatedResult.DefaultPage;

            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Descending = descending.GetValueOrDefault();
            this.SortType = sortType ?? AuthorCollectionSorType.ByTitle.Value;
        }
    }
}