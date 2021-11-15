using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.PublisherCycles
{
    internal class ArchivePublisherCycleHandler : ICommandHandler<ArchivePublisherCycleInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherCycleRepository _publisherCycleRepository;

        public ArchivePublisherCycleHandler(
            IUnitOfWork unitOfWork,
            IPublisherCycleRepository publisherCycleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._publisherCycleRepository = publisherCycleRepository;
        }

        public async Task HandleAsync(ArchivePublisherCycleInternalCommand command)
        {
            var cycle = await this._publisherCycleRepository.GetByCycleGuidAsync(command.PublisherCycleGuid);

            cycle.ArchiveAggregate();

            await this._unitOfWork.CommitAsync(cycle);
        }
    }
}