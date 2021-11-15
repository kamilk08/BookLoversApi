using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Application.Commands.Bookcases;

namespace BookLovers.Bookcases.Application.Integration
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
            var command = new CreateBookcaseInternalCommand(@event.BookcaseGuid, @event.ReaderGuid, @event.ReaderId);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}