using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.TicketOwners;
using BookLovers.Librarians.Events.Tickets;

namespace BookLovers.Librarians.Application.Events
{
    internal class TicketSolvedEventHandler : IDomainEventHandler<TicketSolved>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public TicketSolvedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(TicketSolved @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new NotifyTicketOwnerInternalCommand(@event.AggregateGuid, @event.Notification));
        }
    }
}