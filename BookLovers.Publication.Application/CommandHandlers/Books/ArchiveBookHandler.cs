using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class ArchiveBookHandler : ICommandHandler<ArchiveBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Book> _manager;
        private readonly IInMemoryEventBus _eventBus;

        public ArchiveBookHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Book> manager,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._manager = manager;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(ArchiveBookCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            this._manager.Archive(book);

            await this._unitOfWork.CommitAsync(book);

            await this._eventBus.Publish(new BookArchivedIntegrationEvent(book.Guid));
        }
    }
}