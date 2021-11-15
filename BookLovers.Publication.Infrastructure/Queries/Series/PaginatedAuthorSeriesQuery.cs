using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class PaginatedAuthorSeriesQuery : IQuery<PaginatedResult<SeriesDto>>
    {
        public int AuthorId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public PaginatedAuthorSeriesQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public PaginatedAuthorSeriesQuery(int authorId, int? page, int? count)
        {
            this.AuthorId = authorId;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}