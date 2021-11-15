using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class FindPublisherQuery : IQuery<PaginatedResult<PublisherDto>>
    {
        public string Value { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public FindPublisherQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public FindPublisherQuery(string value, int? page, int? count)
        {
            this.Value = value;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}