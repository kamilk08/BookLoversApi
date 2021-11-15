using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class ChangeBookSeriesHandler : ICommandHandler<ChangeBookSeriesInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeBookSeriesHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeBookSeriesInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.ChangeSeries(new BookSeries(command.SeriesGuid, command.PositionInSeries));

            await this._unitOfWork.CommitAsync(book, false);
        }
    }
}