using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using BookLovers.Ratings.Application.Commands.BookSeries;

namespace BookLovers.Ratings.Application.IntegrationEvents.Series
{
    internal class SeriesHasNewBookHandler :
        IIntegrationEventHandler<SeriesHasNewBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SeriesHasNewBookHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SeriesHasNewBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddSeriesBookInternalCommand(@event.SeriesGuid, @event.BookGuid));
        }
    }
}