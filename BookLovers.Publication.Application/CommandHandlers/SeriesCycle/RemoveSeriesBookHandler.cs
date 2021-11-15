using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Integration.ApplicationEvents.Series;

namespace BookLovers.Publication.Application.CommandHandlers.SeriesCycle
{
    internal class RemoveSeriesBookHandler : ICommandHandler<RemoveSeriesBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public RemoveSeriesBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(RemoveSeriesBookInternalCommand command)
        {
            if (!IfBookHasSeries(command))
                return;

            var series = await this._unitOfWork.GetAsync<Series>(
                command.SeriesGuid.Value);

            var seriesBook = series.GetBook(command.BookGuid);

            series.RemoveBook(seriesBook);

            await this._unitOfWork.CommitAsync(series, false);

            await this._inMemoryEventBus.Publish(new SeriesLostBookIntegrationEvent(series.Guid, seriesBook.BookGuid));
        }

        private bool IfBookHasSeries(RemoveSeriesBookInternalCommand command)
        {
            return command.SeriesGuid.HasValue && command.SeriesGuid.Value != Guid.Empty;
        }
    }
}