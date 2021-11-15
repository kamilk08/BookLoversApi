using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.SeriesCycle
{
    internal class SeriesPositionChangedEventHandler :
        IDomainEventHandler<BookSeriesPositionChanged>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SeriesPositionChangedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookSeriesPositionChanged @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ChangeBookSeriesPositionInternalCommand(@event.SeriesGuid, @event.AggregateGuid, @event.Position));
        }
    }
}