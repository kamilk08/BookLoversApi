using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class PaginatedPublishersCollectionQuery : IQuery<PaginatedResult<int>>
    {
        public int PublisherId { get; set; }

        public string Title { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public bool Descending { get; set; }

        public int SortType { get; set; }

        public PaginatedPublishersCollectionQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            int sortType = this.SortType == 0 ? PublisherCollectionSortingType.ByTitle.Value : this.SortType;

            this.Page = page;
            this.Count = count;
            this.SortType = sortType;
        }

        public PaginatedPublishersCollectionQuery(
            int publisherId,
            string title,
            bool descending,
            int? page,
            int? count,
            int? sortType)
        {
            this.PublisherId = publisherId;
            this.Title = title;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.SortType = sortType ?? PublisherCollectionSortingType.ByTitle.Value;
            this.Descending = descending;
        }
    }
}