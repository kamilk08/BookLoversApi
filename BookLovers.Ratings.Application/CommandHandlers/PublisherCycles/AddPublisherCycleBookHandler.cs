using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Application.CommandHandlers.PublisherCycles
{
    internal class AddPublisherCycleBookHandler : ICommandHandler<AddPublisherCycleBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherCycleRepository _publisherCycleRepository;
        private readonly IBookRepository _bookRepository;

        public AddPublisherCycleBookHandler(
            IUnitOfWork unitOfWork,
            IPublisherCycleRepository publisherCycleRepository,
            IBookRepository bookRepository)
        {
            this._unitOfWork = unitOfWork;
            this._publisherCycleRepository = publisherCycleRepository;
            this._bookRepository = bookRepository;
        }

        public async Task HandleAsync(AddPublisherCycleBookInternalCommand command)
        {
            var cycle = await this._publisherCycleRepository.GetByCycleGuidAsync(command.PublisherCycleGuid);
            var book = await this._bookRepository.GetByBookGuidAsync(command.BookGuid);

            cycle.AddBook(book);

            await this._unitOfWork.CommitAsync(cycle);
        }
    }
}