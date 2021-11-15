using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class RemovePublisherBookHandler : ICommandHandler<RemovePublisherBookInternalCommand>
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemovePublisherBookHandler(
            IPublisherRepository publisherRepository,
            IUnitOfWork unitOfWork)
        {
            _publisherRepository = publisherRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemovePublisherBookInternalCommand command)
        {
            var publisher = await _publisherRepository.GetByPublisherGuidAsync(command.PublisherGuid);

            var book = publisher.Books.SingleOrDefault(p => p.Identification.BookGuid == command.BookGuid);

            if (book != null)
                publisher.RemoveBook(book);

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}