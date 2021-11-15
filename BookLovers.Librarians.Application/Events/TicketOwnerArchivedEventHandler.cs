using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Events.TicketOwners;

namespace BookLovers.Librarians.Application.Events
{
    internal class TicketOwnerArchivedEventHandler :
        IDomainEventHandler<TicketOwnerArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public TicketOwnerArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(TicketOwnerArchived @event)
        {
            var tasks = @event.UnSolvedTickets
                .Select(guid => _commandDispatcher.SendInternalCommandAsync(new ArchiveTicketInternalCommand(guid)));

            return Task.WhenAll(tasks);
        }
    }
}