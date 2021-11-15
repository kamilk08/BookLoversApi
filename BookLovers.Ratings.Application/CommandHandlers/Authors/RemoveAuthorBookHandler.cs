using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Domain.Authors;

namespace BookLovers.Ratings.Application.CommandHandlers.Authors
{
    internal class RemoveAuthorBookHandler : ICommandHandler<RemoveAuthorBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorRepository _authorRepository;

        public RemoveAuthorBookHandler(IUnitOfWork unitOfWork, IAuthorRepository authorRepository)
        {
            this._unitOfWork = unitOfWork;
            this._authorRepository = authorRepository;
        }

        public async Task HandleAsync(RemoveAuthorBookInternalCommand command)
        {
            var author = await this._authorRepository.GetByAuthorGuidAsync(command.AuthorGuid);
            var book = author.Books.SingleOrDefault(p => p.Identification.BookGuid == command.BookGuid);

            author.RemoveBook(book);

            await this._unitOfWork.CommitAsync(author);
        }
    }
}