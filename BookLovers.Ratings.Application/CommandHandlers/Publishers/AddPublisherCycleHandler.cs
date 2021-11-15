using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class AddPublisherCycleHandler : ICommandHandler<AddPublisherCycleInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IPublisherCycleRepository _publisherCycleRepository;

        public AddPublisherCycleHandler(
            IUnitOfWork unitOfWork,
            IPublisherRepository publisherRepository,
            IPublisherCycleRepository publisherCycleRepository)
        {
            _unitOfWork = unitOfWork;
            _publisherRepository = publisherRepository;
            _publisherCycleRepository = publisherCycleRepository;
        }

        public async Task HandleAsync(AddPublisherCycleInternalCommand command)
        {
            var publisher = await _publisherRepository.GetByPublisherGuidAsync(command.PublisherGuid);
            var cycle = await _publisherCycleRepository.GetByCycleGuidAsync(command.PublisherCycleGuid);

            publisher.AddCycle(cycle);

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}