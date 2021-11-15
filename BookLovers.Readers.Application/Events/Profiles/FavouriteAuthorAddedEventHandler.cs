using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Application.Events.Profiles
{
    internal class FavouriteAuthorAddedEventHandler :
        IDomainEventHandler<FavouriteAuthorAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public FavouriteAuthorAddedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(FavouriteAuthorAdded @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new AddFavouriteOwnerInternalCommand(@event.AuthorGuid, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new AddFavouriteAuthorActivityInternalCommand(@event.ReaderGuid, @event.AuthorGuid));
        }
    }
}