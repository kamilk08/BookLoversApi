using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Ratings
{
    public class ReaderRatingsByIdQuery : IQuery<PaginatedResult<RatingDto>>
    {
        public int ReaderId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public ReaderRatingsByIdQuery()
        {
            int page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            int count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public ReaderRatingsByIdQuery(int readerId, int? page, int? count)
        {
            this.ReaderId = readerId;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}