using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;

namespace BookLovers.Librarians.Infrastructure.Queries
{
    public class TicketOwnerByIdQuery : IQuery<TicketOwnerDto>
    {
        public int ReaderId { get; }

        public TicketOwnerByIdQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}