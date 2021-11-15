using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Application.Commands.TicketOwners;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    public class AccountSuspendedHandler :
        IIntegrationEventHandler<UserBlockedIntegrationEvent>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AccountSuspendedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(UserBlockedIntegrationEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new SuspendTicketOwnerInternalCommand(@event.UserGuid));

            if (!@event.IsLibrarian)
                return;

            await this._commandDispatcher.SendInternalCommandAsync(
                new SuspendLibrarianInternalCommand(@event.UserGuid));
        }
    }
}