using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.SeriesCycle
{
    internal class BookHasNoSeriesEventHandler :
        IDomainEventHandler<BookHasNoSeries>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookHasNoSeriesEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookHasNoSeries @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveSeriesBookInternalCommand(@event.OldSeriesGuid, @event.AggregateGuid));
        }
    }
}