using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Readers;

namespace BookLovers.Readers.Application.Integration.Readers
{
    internal class ReaderChangeEmailHandler :
        IIntegrationEventHandler<UserChangedEmailIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderChangeEmailHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserChangedEmailIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new ChangeReaderEmailInternalCommand(@event.ReaderGuid, @event.Email));
        }
    }
}