using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.SeriesCycle;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class BookRemovedFromSeriesEventHandler :
        IDomainEventHandler<BookRemovedFromSeries>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromSeriesEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookRemovedFromSeries @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveBookFromSeriesInternalCommand(@event.BookGuid));
        }
    }
}