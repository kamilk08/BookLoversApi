using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration.Readers
{
    internal class ShelfCreatedHandler :
        IIntegrationEventHandler<ShelfCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ShelfCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ShelfCreatedIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.Shelves.Value,
                    StatisticStep.Increase.Value));
        }
    }
}