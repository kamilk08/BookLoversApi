using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Books;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Application.CommandHandlers.Books
{
    internal class ArchiveBookHandler : ICommandHandler<ArchiveBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;

        public ArchiveBookHandler(IUnitOfWork unitOfWork, IBookRepository bookRepository)
        {
            this._unitOfWork = unitOfWork;
            this._bookRepository = bookRepository;
        }

        public async Task HandleAsync(ArchiveBookInternalCommand command)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(command.BookGuid);

            book.ArchiveAggregate();

            await this._unitOfWork.CommitAsync(book);
        }
    }
}