using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Books;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Application.CommandHandlers.Books
{
    internal class CreateBookHandler : ICommandHandler<CreateBookInternalCommand>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            this._authorRepository = authorRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateBookInternalCommand command)
        {
            var authors = await this._authorRepository.GetMultipleAuthorsAsync(command.AuthorsGuides);
            var book = Book.Create(new BookIdentification(command.BookGuid, command.BookId), authors);

            var @event = BookCreatedEvent.Initialize()
                .WithAggregate(book.Guid)
                .WithBook(book.Identification.BookGuid)
                .WithAuthors(authors.Select(s => s.Identification.AuthorGuid))
                .WithPublisher(command.PublisherGuid)
                .WithSeries(command.SeriesGuid)
                .WithCycles(command.CyclesGuides);

            book.AddEvent(@event);

            await this._unitOfWork.CommitAsync(book);
        }
    }
}