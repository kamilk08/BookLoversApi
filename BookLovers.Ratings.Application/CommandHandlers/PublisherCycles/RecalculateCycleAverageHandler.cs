using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.PublisherCycles
{
    internal class RecalculateCycleAverageHandler :
        ICommandHandler<RecalculateCyclesAverageInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherCycleRepository _repository;

        public RecalculateCycleAverageHandler(
            IUnitOfWork unitOfWork,
            IPublisherCycleRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public async Task HandleAsync(RecalculateCyclesAverageInternalCommand command)
        {
            var cycles = await this._repository.GetCyclesWithBookAsync(command.CycleGuid);

            foreach (var aggregate in cycles)
                await this._unitOfWork.CommitAsync(aggregate);
        }
    }
}