using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using BookLovers.Ratings.Application.Commands.BookSeries;

namespace BookLovers.Ratings.Application.IntegrationEvents.Series
{
    internal class SeriesArchivedHandler :
        IIntegrationEventHandler<SeriesArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SeriesArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SeriesArchivedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchiveSeriesInternalCommand(@event.SeriesGuid));
        }
    }
}