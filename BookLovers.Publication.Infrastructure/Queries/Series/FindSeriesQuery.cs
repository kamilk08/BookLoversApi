using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class FindSeriesQuery : IQuery<PaginatedResult<SeriesDto>>
    {
        public string Value { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public FindSeriesQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public FindSeriesQuery(string value, int? page, int? count)
        {
            this.Value = value;

            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}