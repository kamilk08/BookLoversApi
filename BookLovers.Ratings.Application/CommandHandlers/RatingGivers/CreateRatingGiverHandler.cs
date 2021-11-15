using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.RatingGivers;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Application.CommandHandlers.RatingGivers
{
    internal class CreateRatingGiverHandler : ICommandHandler<CreateRatingGiverInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRatingGiverHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateRatingGiverInternalCommand command)
        {
            var ratingGiver = new RatingGiver(Guid.NewGuid(), command.ReaderGuid,
                command.ReaderId);

            await this._unitOfWork.CommitAsync(ratingGiver);
        }
    }
}