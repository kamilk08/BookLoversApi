using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Application.Commands.Publishers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Publishers
{
    internal class NewPublisherCycleHandler :
        IIntegrationEventHandler<PublisherCycleCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public NewPublisherCycleHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(PublisherCycleCreatedIntegrationEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new CreatePublisherCycleInternalCommand(@event.PublisherCycleGuid, @event.PublisherCycleId));

            await this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherCycleInternalCommand(@event.PublisherGuid, @event.PublisherCycleGuid));
        }
    }
}