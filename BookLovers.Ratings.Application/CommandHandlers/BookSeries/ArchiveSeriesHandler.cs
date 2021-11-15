using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Domain.BookSeries;

namespace BookLovers.Ratings.Application.CommandHandlers.BookSeries
{
    internal class ArchiveSeriesHandler : ICommandHandler<ArchiveSeriesInternalCommand>
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArchiveSeriesHandler(ISeriesRepository seriesRepository, IUnitOfWork unitOfWork)
        {
            this._seriesRepository = seriesRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ArchiveSeriesInternalCommand command)
        {
            var series = await this._seriesRepository.GetBySeriesGuidAsync(command.SeriesGuid);

            series.ArchiveAggregate();

            await this._unitOfWork.CommitAsync(series);
        }
    }
}