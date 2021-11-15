using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;

namespace BookLovers.Librarians.Infrastructure.Queries.Librarians
{
    public class ManageableTicketsQuery : IQuery<PaginatedResult<TicketDto>>
    {
        public string Phrase { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public bool Solved { get; set; }

        public ManageableTicketsQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public ManageableTicketsQuery(string phrase, int? page, int? count, bool? solved)
        {
            this.Phrase = phrase;
            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Solved = solved.GetValueOrDefault();
        }
    }
}