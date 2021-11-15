using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using BookLovers.Ratings.Application.Commands.BookSeries;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookRemovedFromSeriesHandler :
        IIntegrationEventHandler<SeriesLostBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromSeriesHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SeriesLostBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveSeriesBookInternalCommand(@event.SeriesGuid, @event.BookGuid));
        }
    }
}