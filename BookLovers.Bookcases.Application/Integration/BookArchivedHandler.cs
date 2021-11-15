using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Application.Commands.BookcaseBooks;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Bookcases.Application.Integration
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
            var command = new ArchiveBookcaseBookInternalCommand(@event.BookGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}