using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Events.Librarians;

namespace BookLovers.Librarians.Application.Events
{
    internal class LibrarianResolvedTicketEventHandler :
        IDomainEventHandler<LibrarianResolvedTicket>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _internalCommandDispatcher;

        public LibrarianResolvedTicketEventHandler(
            IInternalCommandDispatcher internalCommandDispatcher)
        {
            this._internalCommandDispatcher = internalCommandDispatcher;
        }

        public Task HandleAsync(LibrarianResolvedTicket @event)
        {
            return this._internalCommandDispatcher
                .SendInternalCommandAsync(
                    new SolveTicketInternalCommand(@event.TicketGuid, @event.AggregateGuid, @event.Decision,
                        @event.Justification));
        }
    }
}