using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Readers;

namespace BookLovers.Readers.Application.Integration.Readers
{
    internal class SuperAdminCreatedHandler :
        IIntegrationEventHandler<SuperAdminCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SuperAdminCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SuperAdminCreatedIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(new CreateReaderInternalCommand(
                @event.ReaderGuid,
                @event.SocialProfileGuid, @event.ReaderId, @event.UserName, @event.Email));
        }
    }
}