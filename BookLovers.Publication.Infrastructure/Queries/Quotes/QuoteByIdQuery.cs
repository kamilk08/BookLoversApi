using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public class QuoteByIdQuery : IQuery<QuoteDto>
    {
        public int QuoteId { get; set; }

        public QuoteByIdQuery()
        {
        }

        public QuoteByIdQuery(int quoteId)
        {
            this.QuoteId = quoteId;
        }
    }
}