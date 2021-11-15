using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class AddPublisherBookHandler : ICommandHandler<AddPublisherBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;
        private readonly IPublisherRepository _publisherRepository;

        public AddPublisherBookHandler(
            IUnitOfWork unitOfWork,
            IBookRepository bookRepository,
            IPublisherRepository publisherRepository)
        {
            _unitOfWork = unitOfWork;
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
        }

        public async Task HandleAsync(AddPublisherBookInternalCommand command)
        {
            var publisher = await _publisherRepository.GetByPublisherGuidAsync(command.PublisherGuid);
            var book = await _bookRepository.GetByBookGuidAsync(command.BookGuid);

            publisher.AddBook(book);

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}