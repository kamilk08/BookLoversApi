using BookLovers.Base.Infrastructure.Queries;
using System;

namespace BookLovers.Librarians.Infrastructure.Queries
{
    public class IsTicketOwnedByUser : IQuery<bool>
    {
        public int TicketId { get; }

        public Guid UserGuid { get; }

        public IsTicketOwnedByUser(int ticketId, Guid userGuid)
        {
            this.TicketId = ticketId;
            this.UserGuid = userGuid;
        }
    }
}