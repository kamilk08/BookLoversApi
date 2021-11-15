using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Ratings.Application.Commands.Publishers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Publishers
{
    internal class PublisherArchivedHandler :
        IIntegrationEventHandler<PublisherArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherArchivedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchivePublisherInternalCommand(@event.PublisherGuid));
        }
    }
}