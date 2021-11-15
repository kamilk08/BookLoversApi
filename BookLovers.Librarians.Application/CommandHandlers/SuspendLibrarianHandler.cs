using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.CommandHandlers
{
    internal class SuspendLibrarianHandler :
        ICommandHandler<SuspendLibrarianInternalCommand>,
        ICommandHandler<SuspendLibrarianCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILibrarianRepository _librarianRepository;
        private readonly IAggregateManager<Librarian> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public SuspendLibrarianHandler(
            IUnitOfWork unitOfWork,
            ILibrarianRepository librarianRepository,
            IAggregateManager<Librarian> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._librarianRepository = librarianRepository;
            this._aggregateManager = aggregateManager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(SuspendLibrarianInternalCommand command)
        {
            var librarian = await this._librarianRepository.GetLibrarianByReaderGuid(command.UserGuid);

            this._aggregateManager.Archive(librarian);

            await this._unitOfWork.CommitAsync(librarian);
        }

        public async Task HandleAsync(SuspendLibrarianCommand command)
        {
            var librarian = await this._librarianRepository.GetLibrarianByReaderGuid(command.UserGuid);
            this._aggregateManager.Archive(librarian);

            await this._unitOfWork.CommitAsync(librarian);

            var @event = new LibrarianDegradedToReader(command.UserGuid);

            await this._eventBus.Publish(@event);
        }
    }
}