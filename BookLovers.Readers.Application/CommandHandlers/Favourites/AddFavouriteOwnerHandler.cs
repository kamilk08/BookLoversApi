using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Domain.Favourites;

namespace BookLovers.Readers.Application.CommandHandlers.Favourites
{
    internal class AddFavouriteOwnerHandler : ICommandHandler<AddFavouriteOwnerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFavouriteOwnerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddFavouriteOwnerInternalCommand command)
        {
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.FavouriteGuid);

            favourite.AddFavouriteOwner(command.OwnerGuid);

            await _unitOfWork.CommitAsync(favourite);
        }
    }
}