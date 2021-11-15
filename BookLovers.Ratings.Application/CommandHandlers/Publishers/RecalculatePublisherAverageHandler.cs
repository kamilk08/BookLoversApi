using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class RecalculatePublisherAverageHandler :
        ICommandHandler<RecalculatePublisherAverageInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherRepository _repository;

        public RecalculatePublisherAverageHandler(
            IUnitOfWork unitOfWork,
            IPublisherRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task HandleAsync(RecalculatePublisherAverageInternalCommand command)
        {
            var publisher = await _repository.GetPublisherWithBookAsync(command.BookGuid);

            if (publisher == null)
                return;

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}