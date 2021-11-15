using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.TicketOwners
{
    internal class IsTicketOwnedByUserHandler : IQueryHandler<IsTicketOwnedByUser, bool>
    {
        private readonly LibrariansContext _context;

        public IsTicketOwnedByUserHandler(LibrariansContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(IsTicketOwnedByUser query)
        {
            return this._context.Tickets.AsNoTracking()
                .AnyAsync(a => a.Id == query.TicketId && a.TicketOwnerGuid == query.UserGuid);
        }
    }
}