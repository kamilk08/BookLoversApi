using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.Publishers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookRemovedFromPublisherHandler :
        IIntegrationEventHandler<PublisherLostBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromPublisherHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherLostBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherBookInternalCommand(@event.PublisherGuid, @event.BookGuid));
        }
    }
}