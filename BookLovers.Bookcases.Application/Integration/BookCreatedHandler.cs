using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Application.Commands.BookcaseBooks;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Bookcases.Application.Integration
{
    internal class BookCreatedIntegrationHandler :
        IIntegrationEventHandler<BookCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedIntegrationHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookCreatedIntegrationEvent @event)
        {
            var command = new AddBookcaseBookInternalCommand(@event.BookGuid, @event.BookId);

            return this._commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}