using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public class PaginatedUserQuotesQuery : IQuery<PaginatedResult<QuoteDto>>
    {
        public int ReaderId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public int Order { get; set; }

        public bool Descending { get; set; }

        public PaginatedUserQuotesQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            var order = this.Order == 0 ? QuotesOrder.ByLikes.Value : this.Order;

            this.Page = page;
            this.Count = count;
            this.Order = order;
        }

        public PaginatedUserQuotesQuery(
            int readerId,
            int? page,
            int? count,
            int? order,
            bool? descending)
        {
            this.ReaderId = readerId;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Order = order ?? QuotesOrder.ByLikes.Value;
            this.Descending = descending.GetValueOrDefault();
        }
    }
}