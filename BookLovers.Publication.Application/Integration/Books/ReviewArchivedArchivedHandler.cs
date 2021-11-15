using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Publication.Application.Integration.Books
{
    internal class ReviewArchivedArchivedHandler :
        IIntegrationEventHandler<ReviewArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewArchivedArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewArchivedIntegrationEvent @event)
        {
            var command = new RemoveBookReviewInternalCommand(@event.BookGuid, @event.ReaderGuid);

            return this._commandDispatcher.SendInternalCommandAsync<RemoveBookReviewInternalCommand>(command);
        }
    }
}