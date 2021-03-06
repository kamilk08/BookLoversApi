using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Readers.Application.Commands.Favourites;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookArchivedHandler :
        IIntegrationEventHandler<BookArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookArchivedIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(new ArchiveFavouriteInternalCommand(@event.BookGuid));
        }
    }
}