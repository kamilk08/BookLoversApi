using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class PaginatedPublishersQuery : IQuery<PaginatedResult<PublisherDto>>
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public PaginatedPublishersQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public PaginatedPublishersQuery(int? page, int? count)
        {
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}