using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Application.Commands.BookReaders;

namespace BookLovers.Publication.Application.Integration
{
    internal class UserBlockedHandler :
        IIntegrationEventHandler<UserBlockedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserBlockedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserBlockedIntegrationEvent @event)
        {
            var command = new SuspendBookReaderInternalCommand(@event.UserGuid);

            return this._commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}