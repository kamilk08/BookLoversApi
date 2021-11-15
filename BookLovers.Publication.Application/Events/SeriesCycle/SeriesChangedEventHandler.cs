using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.SeriesCycle
{
    internal class SeriesChangedEventHandler : IDomainEventHandler<SeriesChanged>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SeriesChangedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(SeriesChanged @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new RemoveSeriesBookInternalCommand(@event.OldSeriesGuid, @event.AggregateGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new AddSeriesBookInternalCommand(@event.SeriesGuid, @event.PositionInSeries, @event.AggregateGuid));
        }
    }
}