using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using BookLovers.Ratings.Application.Commands.BookSeries;

namespace BookLovers.Ratings.Application.IntegrationEvents.Series
{
    internal class NewSeriesAddedHandler :
        IIntegrationEventHandler<NewSeriesAddedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public NewSeriesAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(NewSeriesAddedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new CreateSeriesInternalCommand(@event.SeriesGuid, @event.SeriesId));
        }
    }
}