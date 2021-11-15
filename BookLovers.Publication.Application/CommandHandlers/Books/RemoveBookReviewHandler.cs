using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class RemoveBookReviewHandler : ICommandHandler<RemoveBookReviewInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveBookReviewHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveBookReviewInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);
            var bookReview = book.GetBookReview(command.ReaderGuid);

            book.RemoveReview(bookReview);

            await this._unitOfWork.CommitAsync(book);
        }
    }
}