using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Ratings.Application.Commands.Books;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookArchivedHandler :
        IIntegrationEventHandler<BookArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookArchivedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchiveBookInternalCommand(@event.BookGuid));
        }
    }
}