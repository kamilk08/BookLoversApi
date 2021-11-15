using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Events.Tickets;

namespace BookLovers.Librarians.Application.Events
{
    public class TicketCreatedEventHandler : IDomainEventHandler<TicketCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public TicketCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(TicketCreated @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddTicketToTheOwnerInternalCommand(@event.AggregateGuid, @event.TicketOwnerGuid));
        }
    }
}