using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class RemoveFavouriteHandler : ICommandHandler<RemoveFavouriteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FavouritesService _favouritesService;

        public RemoveFavouriteHandler(IUnitOfWork unitOfWork, FavouritesService favouritesService)
        {
            _unitOfWork = unitOfWork;
            _favouritesService = favouritesService;
        }

        public async Task HandleAsync(RemoveFavouriteCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.WriteModel.ProfileGuid);

            if (!profile.HasFavourite(command.WriteModel.FavouriteGuid))
                return;

            var favourite = profile.GetFavourite(command.WriteModel.FavouriteGuid);

            _favouritesService.RemoveFavourite(profile, favourite);

            await _unitOfWork.CommitAsync(profile);
        }
    }
}