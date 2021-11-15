using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class RemoveBookFromSeriesHandler : ICommandHandler<RemoveBookFromSeriesInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveBookFromSeriesHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveBookFromSeriesInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.ChangeSeries(new BookSeries(null, null));

            await this._unitOfWork.CommitAsync(book, false);
        }
    }
}