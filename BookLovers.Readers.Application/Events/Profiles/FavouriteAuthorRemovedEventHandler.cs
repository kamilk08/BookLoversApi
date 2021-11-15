using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Application.Events.Profiles
{
    internal class FavouriteAuthorRemovedEventHandler :
        IDomainEventHandler<FavouriteAuthorRemoved>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public FavouriteAuthorRemovedEventHandler(
            IInternalCommandDispatcher commandDispatcher,
            IInMemoryEventBus inMemoryEventBus)
        {
            _commandDispatcher = commandDispatcher;
            _inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(FavouriteAuthorRemoved @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new RemoveFavouriteOwnerInternalCommand(@event.AuthorGuid, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new AddActivityOfTypeRemoveFavouriteAuthorInternalCommand(@event.ReaderGuid, @event.AuthorGuid));
        }
    }
}