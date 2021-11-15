using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.PublisherCycles;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookRemovedFromPublisherCycleHandler :
        IIntegrationEventHandler<BookRemovedFromPublisherCycleIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromPublisherCycleHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(
            BookRemovedFromPublisherCycleIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherCycleBookInternalCommand(@event.PublisherCycleGuid, @event.BookGuid));
        }
    }
}