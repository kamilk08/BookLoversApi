using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.PublisherCycles
{
    internal class CreatePublisherCycleHandler : ICommandHandler<CreatePublisherCycleInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePublisherCycleHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreatePublisherCycleInternalCommand command)
        {
            var cycle = PublisherCycle.Create(new PublisherCycleIdentification(
                command.PublisherCycleGuid,
                command.PublisherCycleId));

            return this._unitOfWork.CommitAsync(cycle);
        }
    }
}