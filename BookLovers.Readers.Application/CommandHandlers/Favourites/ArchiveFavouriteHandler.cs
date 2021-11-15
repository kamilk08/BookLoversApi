using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Domain.Favourites;

namespace BookLovers.Readers.Application.CommandHandlers.Favourites
{
    internal class ArchiveFavouriteHandler : ICommandHandler<ArchiveFavouriteInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Favourite> _aggregateManager;

        public ArchiveFavouriteHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Favourite> aggregateManager)
        {
            _unitOfWork = unitOfWork;
            _aggregateManager = aggregateManager;
        }

        public async Task HandleAsync(ArchiveFavouriteInternalCommand command)
        {
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.FavouriteGuid);

            _aggregateManager.Archive(favourite);

            await _unitOfWork.CommitAsync(favourite);
        }
    }
}