using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.PublisherCycles
{
    internal class RemovePublisherCycleBookHandler :
        ICommandHandler<RemovePublisherCycleBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherCycleRepository _publisherCycleRepository;

        public RemovePublisherCycleBookHandler(
            IUnitOfWork unitOfWork,
            IPublisherCycleRepository publisherCycleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._publisherCycleRepository = publisherCycleRepository;
        }

        public async Task HandleAsync(RemovePublisherCycleBookInternalCommand command)
        {
            var cycle = await this._publisherCycleRepository.GetByCycleGuidAsync(command.PublisherCycleGuid);
            var book = cycle.Books.SingleOrDefault(p => p.Identification.BookGuid == command.BookGuid);

            if (book != null)
                cycle.RemoveBook(book);

            await this._unitOfWork.CommitAsync(cycle);
        }
    }
}