using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket> GetTicketByIdAsync(int ticketId);
    }
}