using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Application.CommandHandlers.Authors
{
    internal class AddAuthorBookHandler : ICommandHandler<AddAuthorBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public AddAuthorBookHandler(
            IUnitOfWork unitOfWork,
            IAuthorRepository authorRepository,
            IBookRepository bookRepository)
        {
            this._unitOfWork = unitOfWork;
            this._authorRepository = authorRepository;
            this._bookRepository = bookRepository;
        }

        public async Task HandleAsync(AddAuthorBookInternalCommand command)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(command.BookGuid);

            var author = await this._authorRepository.GetByAuthorGuidAsync(command.AuthorGuid);

            author.AddBook(book);

            await this._unitOfWork.CommitAsync(author);
        }
    }
}