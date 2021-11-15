using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;

namespace BookLovers.Librarians.Infrastructure.Queries.Tickets
{
    public class TicketByIdQuery : IQuery<TicketDto>
    {
        public int TicketId { get; set; }

        public TicketByIdQuery()
        {
        }

        public TicketByIdQuery(int ticketId)
        {
            this.TicketId = ticketId;
        }
    }
}