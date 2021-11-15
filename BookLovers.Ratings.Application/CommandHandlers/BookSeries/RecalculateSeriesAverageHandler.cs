using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Domain.BookSeries;

namespace BookLovers.Ratings.Application.CommandHandlers.BookSeries
{
    internal class RecalculateSeriesAverageHandler :
        ICommandHandler<RecalculateSeriesAverageInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeriesRepository _repository;

        public RecalculateSeriesAverageHandler(IUnitOfWork unitOfWork, ISeriesRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public async Task HandleAsync(
            RecalculateSeriesAverageInternalCommand internalCommand)
        {
            var series = await this._repository.GetSeriesWithBookAsync(internalCommand.BookGuid);

            foreach (var aggregate in series)
                await this._unitOfWork.CommitAsync(aggregate);
        }
    }
}