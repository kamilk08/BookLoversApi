using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class RemovePublisherCycleHandler : ICommandHandler<RemovePublisherCycleInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherRepository _publisherRepository;

        public RemovePublisherCycleHandler(
            IUnitOfWork unitOfWork,
            IPublisherRepository publisherRepository)
        {
            _unitOfWork = unitOfWork;
            _publisherRepository = publisherRepository;
        }

        public async Task HandleAsync(RemovePublisherCycleInternalCommand command)
        {
            var publisher = await _publisherRepository.GetByPublisherGuidAsync(command.PublisherGuid);
            var cycle = publisher.GetPublisherCycle(command.PublisherCycleGuid);

            publisher.RemoveCycle(cycle);

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}