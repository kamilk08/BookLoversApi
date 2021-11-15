using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.RatingGivers;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Application.CommandHandlers.RatingGivers
{
    internal class ArchiveRatingGiverHandler : ICommandHandler<ArchiveRatingGiverInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRatingGiverRepository _repository;
        private readonly IAggregateManager<RatingGiver> _manager;

        public ArchiveRatingGiverHandler(
            IUnitOfWork unitOfWork,
            IRatingGiverRepository repository,
            IAggregateManager<RatingGiver> manager)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._manager = manager;
        }

        public async Task HandleAsync(ArchiveRatingGiverInternalCommand command)
        {
            var ratingGiver = await this._repository.GetRatingGiverByReaderGuid(command.ReaderGuid);

            this._manager.Archive(ratingGiver);

            await this._unitOfWork.CommitAsync<RatingGiver>(ratingGiver);
        }
    }
}