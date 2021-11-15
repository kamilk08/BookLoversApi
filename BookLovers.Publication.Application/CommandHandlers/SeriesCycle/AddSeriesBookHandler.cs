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
    internal class AddSeriesBookHandler : ICommandHandler<AddSeriesBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddSeriesBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddSeriesBookInternalCommand command)
        {
            if (!this.IfBookHasSeries(command))
                return;

            var series = await this._unitOfWork.GetAsync<Series>(
                command.SeriesGuid.Value);

            series?.AddToSeries(new SeriesBook(
                command.BookGuid,
                command.PositionInSeries.Value));

            await this._unitOfWork.CommitAsync(series, false);

            if (command.SendIntegrationEvent)
                await this._inMemoryEventBus.Publish(
                    new SeriesHasNewBookIntegrationEvent(series.Guid, command.BookGuid));
        }

        private bool IfBookHasSeries(AddSeriesBookInternalCommand command)
        {
            return command.SeriesGuid.HasValue && command.SeriesGuid != Guid.Empty;
        }
    }
}