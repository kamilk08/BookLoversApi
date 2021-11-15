using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.BookSeries;

namespace BookLovers.Ratings.Application.CommandHandlers.BookSeries
{
    internal class AddSeriesBookHandler : ICommandHandler<AddSeriesBookInternalCommand>
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddSeriesBookHandler(
            ISeriesRepository seriesRepository,
            IBookRepository bookRepository,
            IUnitOfWork unitOfWork)
        {
            this._seriesRepository = seriesRepository;
            this._bookRepository = bookRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddSeriesBookInternalCommand command)
        {
            var series = await this._seriesRepository.GetBySeriesGuidAsync(command.SeriesGuid);
            var book = await this._bookRepository.GetByBookGuidAsync(command.BookGuid);

            series.AddBook(book);

            await this._unitOfWork.CommitAsync(series);
        }
    }
}