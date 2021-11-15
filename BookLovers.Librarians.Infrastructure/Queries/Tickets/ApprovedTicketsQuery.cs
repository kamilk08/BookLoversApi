using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Queries.Tickets
{
    public class ApprovedTicketsQuery : IQuery<IList<TicketDto>>
    {
        public int LibrarianId { get; }

        public ApprovedTicketsQuery(int librarianId)
        {
            this.LibrarianId = librarianId;
        }
    }
}