using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Events.Shelf;

namespace BookLovers.Bookcases.Application.Events.Bookcases
{
    internal class BookShelfChangedEventHandler :
        IDomainEventHandler<BookShelfChanged>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookShelfChangedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookShelfChanged @event)
        {
            var command = new ReTrackBookInternalCommand(@event.TrackerGuid, @event.OldShelfGuid, @event.NewShelfGuid,
                @event.BookGuid, @event.ChangedAt);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}