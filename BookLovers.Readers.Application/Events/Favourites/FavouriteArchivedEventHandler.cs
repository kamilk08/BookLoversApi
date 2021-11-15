using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Events.Favourites;

namespace BookLovers.Readers.Application.Events.Favourites
{
    internal class FavouriteArchivedEventHandler :
        IDomainEventHandler<FavouriteArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteArchivedEventHandler(
            IInternalCommandDispatcher commandDispatcher,
            IUnitOfWork unitOfWork)
        {
            _commandDispatcher = commandDispatcher;
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(FavouriteArchived @event)
        {
            return Task.WhenAll(@event.FavouriteOwners.Select(favouriteOwner =>
                RemoveFavouriteAsync(@event, favouriteOwner)));
        }

        private async Task RemoveFavouriteAsync(FavouriteArchived @event, Guid favouriteOwnerGuid)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(favouriteOwnerGuid);

            await _commandDispatcher.SendInternalCommandAsync(new RemoveFavouriteInternalCommand(
                @event.AggregateGuid,
                reader.Socials.ProfileGuid));
        }
    }
}