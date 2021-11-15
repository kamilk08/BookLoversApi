using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class AddQuoteToBookHandler : ICommandHandler<AddQuoteToBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddQuoteToBookHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddQuoteToBookInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.AddBookQuote(new BookQuote(command.QuoteGuid));

            await this._unitOfWork.CommitAsync(book);
        }
    }
}