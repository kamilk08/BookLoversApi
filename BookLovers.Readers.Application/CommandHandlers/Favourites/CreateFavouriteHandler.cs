using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Domain.Favourites;

namespace BookLovers.Readers.Application.CommandHandlers.Favourites
{
    internal class CreateFavouriteHandler : ICommandHandler<CreateFavouriteInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFavouriteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateFavouriteInternalCommand command)
        {
            var favourite = new Favourite(command.FavouriteGuid, command.FavouriteId,
                command.AddedByGuid);

            return _unitOfWork.CommitAsync(favourite);
        }
    }
}