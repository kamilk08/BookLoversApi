using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Domain.BookSeries;

namespace BookLovers.Ratings.Application.CommandHandlers.BookSeries
{
    internal class CreateSeriesHandler : ICommandHandler<CreateSeriesInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSeriesHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateSeriesInternalCommand command)
        {
            var series = Series.Create(new SeriesIdentification(command.SeriesGuid, command.SeriesId));

            return this._unitOfWork.CommitAsync(series);
        }
    }
}