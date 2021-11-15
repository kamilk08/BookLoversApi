using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using System;

namespace BookLovers.Librarians.Infrastructure.Queries.Tickets
{
    public class TicketByGuidQuery : IQuery<TicketDto>
    {
        public Guid TicketGuid { get; }

        public TicketByGuidQuery(Guid ticketGuid)
        {
            this.TicketGuid = ticketGuid;
        }
    }
}