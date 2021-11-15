using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Application.Commands;

namespace BookLovers.Bookcases.Application.Integration
{
    internal class AccountSuspendedHandler :
        IIntegrationEventHandler<UserBlockedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AccountSuspendedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserBlockedIntegrationEvent @event)
        {
            var command = new ArchiveBookcaseOwnerInternalCommand(@event.UserGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}