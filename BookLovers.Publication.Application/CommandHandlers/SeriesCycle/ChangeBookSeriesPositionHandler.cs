using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Domain.SeriesCycle;

namespace BookLovers.Publication.Application.CommandHandlers.SeriesCycle
{
    internal class ChangeBookSeriesPositionHandler :
        ICommandHandler<ChangeBookSeriesPositionInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeBookSeriesPositionHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeBookSeriesPositionInternalCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Series>(command.SeriesGuid);

            publisher.ChangePosition(new SeriesBook(command.BookGuid, command.Position));

            await this._unitOfWork.CommitAsync(publisher, false);
        }
    }
}