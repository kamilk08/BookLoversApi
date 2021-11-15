using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;

namespace BookLovers.Readers.Application.Integration.Authors
{
    internal class AuthorAcceptedByLibrarianHandler :
        IIntegrationEventHandler<AuthorAcceptedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorAcceptedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher) =>
            _commandDispatcher = commandDispatcher;

        public Task HandleAsync(AuthorAcceptedByLibrarian @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddAuthorAcceptedNotificationInternalCommand(@event.ReaderGuid, @event.AcceptedByGuid,
                    @event.AuthorGuid,
                    @event.Notification));
        }
    }
}