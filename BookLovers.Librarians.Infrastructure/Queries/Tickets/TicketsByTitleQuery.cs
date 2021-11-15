using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;

namespace BookLovers.Librarians.Infrastructure.Queries.Tickets
{
    public class TicketsByTitleQuery : IQuery<PaginatedResult<TicketDto>>
    {
        public string Phrase { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public bool Solved { get; set; }

        public TicketsByTitleQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;
            var str = this.Phrase ?? string.Empty;

            this.Page = page;
            this.Count = count;
            this.Phrase = str;
        }

        public TicketsByTitleQuery(bool solved, int? page, int? count, string phrase = null)
        {
            this.Phrase = phrase ?? string.Empty;
            this.Solved = solved;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
        }
    }
}