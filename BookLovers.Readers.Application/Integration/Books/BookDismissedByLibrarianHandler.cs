using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookDismissedByLibrarianHandler :
        IIntegrationEventHandler<BookDismissedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookDismissedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookDismissedByLibrarian @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddBookDismissedNotificationInternalCommand(@event.ReaderGuid, @event.DismissedByGuid,
                    @event.BookGuid,
                    @event.Justification));
        }
    }
}