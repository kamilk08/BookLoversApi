using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Application.Events.Profiles
{
    internal class FavouriteBookRemovedEventHandler :
        IDomainEventHandler<FavouriteBookRemoved>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public FavouriteBookRemovedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(FavouriteBookRemoved @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new RemoveFavouriteOwnerInternalCommand(@event.BookGuid, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new AddActivityOfTypeRemoveFavouriteBookInternalCommand(@event.ReaderGuid, @event.BookGuid));
        }
    }
}