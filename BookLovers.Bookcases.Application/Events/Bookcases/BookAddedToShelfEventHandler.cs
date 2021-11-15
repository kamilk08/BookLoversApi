using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Application.Events.Bookcases
{
    internal class BookAddedToShelfEventHandler :
        IDomainEventHandler<BookAddedToShelf>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookAddedToShelfEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookAddedToShelf @event)
        {
            var command = new TrackBookInternalCommand(
                @event.TrackerGuid,
                @event.ShelfGuid,
                @event.BookGuid,
                @event.AddedAt);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}