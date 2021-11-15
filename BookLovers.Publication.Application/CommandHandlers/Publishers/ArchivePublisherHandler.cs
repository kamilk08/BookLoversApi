using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class ArchivePublisherHandler : ICommandHandler<ArchivePublisherCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Publisher> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public ArchivePublisherHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Publisher> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._aggregateManager = aggregateManager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(ArchivePublisherCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.PublisherGuid);

            this._aggregateManager.Archive(publisher);

            await this._unitOfWork.CommitAsync(publisher);

            await this._eventBus.Publish(new PublisherArchivedIntegrationEvent(command.PublisherGuid));
        }
    }
}