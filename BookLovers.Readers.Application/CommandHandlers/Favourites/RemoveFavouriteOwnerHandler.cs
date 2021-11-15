using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Domain.Favourites;

namespace BookLovers.Readers.Application.CommandHandlers.Favourites
{
    internal class RemoveFavouriteOwnerHandler : ICommandHandler<RemoveFavouriteOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFavouriteOwnerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveFavouriteOwnerInternalCommand command)
        {
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.FavouriteGuid);

            favourite.RemoveFavouriteOwner(command.OwnerGuid);

            await _unitOfWork.CommitAsync(favourite);
        }
    }
}