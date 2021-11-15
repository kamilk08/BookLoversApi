using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Application.Commands.BookReaders;

namespace BookLovers.Publication.Application.Integration
{
    internal class SuperAdminCreatedHandler :
        IIntegrationEventHandler<SuperAdminCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SuperAdminCreatedHandler(IInternalCommandDispatcher commandDispatcher) =>
            this._commandDispatcher = commandDispatcher;

        public Task HandleAsync(SuperAdminCreatedIntegrationEvent @event)
        {
            var command = new CreateBookReaderInternalCommand(@event.ReaderGuid, @event.ReaderId);

            return this._commandDispatcher.SendInternalCommandAsync<CreateBookReaderInternalCommand>(command);
        }
    }
}