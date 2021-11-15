using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Ratings.Application.Commands.Books;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookCreatedHandler :
        IIntegrationEventHandler<BookCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookCreatedIntegrationEvent @event)
        {
            var command = new CreateBookInternalCommand(@event.BookGuid, @event.BookId, @event.Authors,
                @event.PublisherGuid,
                @event.SeriesGuid, @event.Cycles);

            return this._commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}