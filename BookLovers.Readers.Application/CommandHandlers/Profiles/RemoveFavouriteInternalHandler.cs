using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class RemoveFavouriteInternalHandler : ICommandHandler<RemoveFavouriteInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FavouritesService _favouritesService;

        public RemoveFavouriteInternalHandler(
            IUnitOfWork unitOfWork,
            FavouritesService favouritesService)
        {
            _unitOfWork = unitOfWork;
            _favouritesService = favouritesService;
        }

        public async Task HandleAsync(RemoveFavouriteInternalCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.ProfileGuid);
            if (!profile.HasFavourite(command.FavouriteGuid))
                return;

            var favourite = profile.GetFavourite(command.FavouriteGuid);

            _favouritesService.RemoveFavourite(profile, favourite);

            await _unitOfWork.CommitAsync(profile, false);
        }
    }
}