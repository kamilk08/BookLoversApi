using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Application.Events.Bookcases
{
    internal class BookRemovedFromShelfEventHandler :
        IDomainEventHandler<BookRemovedFromShelf>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromShelfEventHandler(
            IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookRemovedFromShelf @event)
        {
            var command =
                new UnTrackBookInternalCommand(@event.ShelfRecordTrackerGuid, @event.ShelfGuid, @event.BookGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}