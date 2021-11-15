using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Favourites;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class AddFavouriteAuthorHandler : ICommandHandler<AddFavouriteAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly FavouritesService _favouritesService;

        public AddFavouriteAuthorHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IInMemoryEventBus eventBus,
            FavouritesService favouritesService)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _favouritesService = favouritesService;
        }

        public async Task HandleAsync(AddFavouriteAuthorCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.WriteModel.ProfileGuid);
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.WriteModel.AuthorGuid);

            _favouritesService.AddFavourite(profile, new FavouriteAuthor(favourite.FavouriteGuid));

            await _unitOfWork.CommitAsync(profile);
        }
    }
}