using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookAcceptedByLibrarianHandler :
        IIntegrationEventHandler<BookAcceptedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookAcceptedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookAcceptedByLibrarian @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddBookAcceptedNotificationInternalCommand(@event.ReaderGuid, @event.AcceptedByGuid,
                    @event.BookGuid,
                    @event.Notification));
        }
    }
}