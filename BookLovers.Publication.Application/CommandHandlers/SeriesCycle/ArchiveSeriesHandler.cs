using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Integration.ApplicationEvents.Series;

namespace BookLovers.Publication.Application.CommandHandlers.SeriesCycle
{
    internal class ArchiveSeriesHandler : ICommandHandler<ArchiveSeriesCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Series> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public ArchiveSeriesHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Series> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._aggregateManager = aggregateManager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(ArchiveSeriesCommand command)
        {
            var series = await this._unitOfWork.GetAsync<Series>(command.SeriesGuid);

            this._aggregateManager.Archive(series);

            await this._unitOfWork.CommitAsync(series);

            await this._eventBus.Publish(new SeriesArchivedIntegrationEvent(series.Guid));
        }
    }
}