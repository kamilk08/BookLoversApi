using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;

namespace BookLovers.Readers.Application.Integration.Authors
{
    internal class AuthorDismissedByLibrarianHandler :
        IIntegrationEventHandler<AuthorDismissedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorDismissedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorDismissedByLibrarian @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddAuthorDismissedNotificationInternalCommand(@event.ReaderGuid, @event.DismissedByGuid,
                    @event.AuthorGuid,
                    @event.Justification));
        }
    }
}