using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Application.Commands.Bookcases;

namespace BookLovers.Bookcases.Application.Integration
{
    internal class UserSignedUpHandler :
        IIntegrationEventHandler<UserSignedUpIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserSignedUpHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserSignedUpIntegrationEvent @event)
        {
            var command = new CreateBookcaseInternalCommand(@event.BookcaseGuid, @event.ReaderGuid, @event.ReaderId);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}