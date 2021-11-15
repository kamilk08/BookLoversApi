using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class PaginatedSeriesCollectionQuery : IQuery<PaginatedResult<int>>
    {
        public int SeriesId { get; set; }

        public string Title { get; set; }

        public bool Descending { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public int SortType { get; set; }

        public PaginatedSeriesCollectionQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            int sortType = this.SortType == 0 ? SeriesCollectionSortingType.ByTitle.Value : this.SortType;

            this.Page = page;
            this.Count = count;
            this.SortType = sortType;
        }

        public PaginatedSeriesCollectionQuery(
            int seriesId,
            string title,
            int? page,
            int? count,
            bool? descending,
            int? sortType)
        {
            this.SeriesId = seriesId;
            this.Title = title;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Descending = descending.GetValueOrDefault();
            this.SortType = sortType ?? SeriesCollectionSortingType.ByTitle.Value;
        }
    }
}