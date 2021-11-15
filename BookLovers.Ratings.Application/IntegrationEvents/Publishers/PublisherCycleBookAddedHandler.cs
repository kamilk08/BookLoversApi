using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.PublisherCycles;

namespace BookLovers.Ratings.Application.IntegrationEvents.Publishers
{
    internal class PublisherCycleBookAddedHandler :
        IIntegrationEventHandler<PublisherCycleHasNewBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherCycleBookAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherCycleHasNewBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherCycleBookInternalCommand(@event.PublisherCycleGuid, @event.BookGuid));
        }
    }
}