using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Favourites;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class AddFavouriteBookHandler : ICommandHandler<AddFavouriteBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FavouritesService _favouritesService;

        public AddFavouriteBookHandler(IUnitOfWork unitOfWork, FavouritesService favouritesService)
        {
            _unitOfWork = unitOfWork;
            _favouritesService = favouritesService;
        }

        public async Task HandleAsync(AddFavouriteBookCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.WriteModel.ProfileGuid);
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.WriteModel.BookGuid);

            _favouritesService.AddFavourite(profile, new FavouriteBook(favourite.FavouriteGuid));

            await _unitOfWork.CommitAsync(profile);
        }
    }
}