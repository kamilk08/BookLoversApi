using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.PublisherCycles
{
    internal class ArchivePublisherCycleHandler : ICommandHandler<ArchivePublisherCycleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<PublisherCycle> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public ArchivePublisherCycleHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<PublisherCycle> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._aggregateManager = aggregateManager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(ArchivePublisherCycleCommand command)
        {
            var publisherCycle = await this._unitOfWork.GetAsync<PublisherCycle>(command.PublisherCycleGuid);

            this._aggregateManager.Archive(publisherCycle);

            await this._unitOfWork.CommitAsync(publisherCycle);

            await this._eventBus.Publish(
                new PublisherCycleArchivedIntegrationEvent(publisherCycle.Guid, publisherCycle.PublisherGuid));
        }
    }
}