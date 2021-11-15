using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.SeriesCycle
{
    internal class BookCreatedEventHandler : IDomainEventHandler<BookCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookCreated @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddSeriesBookInternalCommand(@event.SeriesGuid, @event.PositionInSeries, @event.AggregateGuid,
                    false));
        }
    }
}