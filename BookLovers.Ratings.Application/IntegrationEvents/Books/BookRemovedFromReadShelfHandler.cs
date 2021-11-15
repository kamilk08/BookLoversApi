using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Ratings.Application.Commands.RatingGivers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookRemovedFromReadShelfHandler :
        IIntegrationEventHandler<BookRemovedFromReadShelfIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromReadShelfHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookRemovedFromReadShelfIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveRatingInternalCommand(@event.ReaderGuid, @event.BookGuid));
        }
    }
}