using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class ArchivePublisherHandler : ICommandHandler<ArchivePublisherInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherRepository _publisherRepository;

        public ArchivePublisherHandler(IUnitOfWork unitOfWork, IPublisherRepository publisherRepository)
        {
            _unitOfWork = unitOfWork;
            _publisherRepository = publisherRepository;
        }

        public async Task HandleAsync(ArchivePublisherInternalCommand command)
        {
            var publisher = await _publisherRepository.GetByPublisherGuidAsync(command.PublisherGuid);

            publisher.ArchiveAggregate();

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}