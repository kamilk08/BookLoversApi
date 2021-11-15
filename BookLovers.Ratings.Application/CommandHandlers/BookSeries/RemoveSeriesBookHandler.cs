using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Domain.BookSeries;

namespace BookLovers.Ratings.Application.CommandHandlers.BookSeries
{
    internal class RemoveSeriesBookHandler : ICommandHandler<RemoveSeriesBookInternalCommand>
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveSeriesBookHandler(ISeriesRepository seriesRepository, IUnitOfWork unitOfWork)
        {
            this._seriesRepository = seriesRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveSeriesBookInternalCommand command)
        {
            var series = await this._seriesRepository.GetBySeriesGuidAsync(command.SeriesGuid);
            var book = series.Books.SingleOrDefault(p => p.Identification.BookGuid == command.BookGuid);

            if (book != null)
                series.RemoveBook(book);

            await this._unitOfWork.CommitAsync(series);
        }
    }
}