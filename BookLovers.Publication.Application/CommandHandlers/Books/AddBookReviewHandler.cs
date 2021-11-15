using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class AddBookReviewHandler : ICommandHandler<AddBookReviewInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookReviewHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddBookReviewInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.AddReview(new BookReview(command.ReaderGuid, command.ReviewGuid));

            await this._unitOfWork.CommitAsync(book);
        }
    }
}