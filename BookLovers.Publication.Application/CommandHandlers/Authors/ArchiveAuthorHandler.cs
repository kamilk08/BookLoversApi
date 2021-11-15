using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class ArchiveAuthorHandler : ICommandHandler<ArchiveAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Author> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public ArchiveAuthorHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Author> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._aggregateManager = aggregateManager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(ArchiveAuthorCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);

            this._aggregateManager.Archive(author);

            await this._unitOfWork.CommitAsync(author);

            await this._eventBus.Publish(new AuthorArchivedIntegrationEvent(author.Guid));
        }
    }
}