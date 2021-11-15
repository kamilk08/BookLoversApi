using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Integration.ApplicationEvents.Series;

namespace BookLovers.Publication.Application.CommandHandlers.SeriesCycle
{
    internal class CreateSeriesHandler : ICommandHandler<CreateSeriesCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly SeriesFactory _factory;

        public CreateSeriesHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            IReadContextAccessor readContextAccessor,
            SeriesFactory factory)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._readContextAccessor = readContextAccessor;
            this._factory = factory;
        }

        public async Task HandleAsync(CreateSeriesCommand command)
        {
            var series = this._factory.Create(command.WriteModel.SeriesGuid, command.WriteModel.SeriesName);

            await this._unitOfWork.CommitAsync(series);

            command.WriteModel.SeriesId = this._readContextAccessor.GetReadModelId(series.Guid);

            await this._eventBus.Publish(new NewSeriesAddedIntegrationEvent(series.Guid, command.WriteModel.SeriesId));
        }
    }
}