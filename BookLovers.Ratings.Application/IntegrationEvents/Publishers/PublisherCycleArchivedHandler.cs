using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Application.Commands.Publishers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Publishers
{
    internal class PublisherCycleArchivedHandler :
        IIntegrationEventHandler<PublisherCycleArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherCycleArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(PublisherCycleArchivedIntegrationEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new ArchivePublisherCycleInternalCommand(@event.PublisherCycleGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherCycleInternalCommand(@event.PublisherGuid, @event.PublisherCycleGuid));
        }
    }
}