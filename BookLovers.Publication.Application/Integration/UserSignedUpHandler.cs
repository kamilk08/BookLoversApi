using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Application.Commands.BookReaders;

namespace BookLovers.Publication.Application.Integration
{
    internal class UserSignedUpHandler :
        IIntegrationEventHandler<UserSignedUpIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserSignedUpHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserSignedUpIntegrationEvent @event)
        {
            var command = new CreateBookReaderInternalCommand(@event.ReaderGuid, @event.ReaderId);

            return this._commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}