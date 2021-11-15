using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Auth.Application.Integration
{
    internal class LibrarianDegradedToReaderHandler :
        IIntegrationEventHandler<LibrarianDegradedToReader>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public LibrarianDegradedToReaderHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(LibrarianDegradedToReader @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new DegradeToReaderInternalCommand(@event.UserGuid));
        }
    }
}