using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.Publishers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Publishers
{
    internal class PublisherHasNewBookHandler :
        IIntegrationEventHandler<PublisherHasNewBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherHasNewBookHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherHasNewBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherBookInternalCommand(@event.PublisherGuid, @event.BookGuid));
        }
    }
}