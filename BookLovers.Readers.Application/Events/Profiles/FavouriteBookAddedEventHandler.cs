using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Application.Events.Profiles
{
    internal class FavouriteBookAddedEventHandler :
        IDomainEventHandler<FavouriteBookAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public FavouriteBookAddedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(FavouriteBookAdded @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new AddFavouriteOwnerInternalCommand(@event.BookGuid, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new AddFavouriteBookActivityInternalCommand(@event.ReaderGuid, @event.BookGuid));
        }
    }
}