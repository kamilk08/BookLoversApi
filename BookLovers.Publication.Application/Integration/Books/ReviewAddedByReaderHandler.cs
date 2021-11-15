using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Publication.Application.Integration.Books
{
    internal class ReviewAddedByReaderHandler :
        IIntegrationEventHandler<ReviewAddedByReaderIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewAddedByReaderHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewAddedByReaderIntegrationEvent @event)
        {
            var command = new AddBookReviewInternalCommand(@event.BookGuid, @event.AggregateGuid, @event.ReviewGuid);

            return this._commandDispatcher.SendInternalCommandAsync<AddBookReviewInternalCommand>(command);
        }
    }
}