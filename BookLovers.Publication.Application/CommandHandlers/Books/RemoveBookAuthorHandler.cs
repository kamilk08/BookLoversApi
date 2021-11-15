using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class RemoveBookAuthorHandler : ICommandHandler<RemoveBookAuthorInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IUnknownAuthorService _unknownAuthorService;

        public RemoveBookAuthorHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            IUnknownAuthorService unknownAuthorService)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._unknownAuthorService = unknownAuthorService;
        }

        public async Task HandleAsync(RemoveBookAuthorInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);
            var bookAuthor = book.GetBookAuthor(command.AuthorGuid);

            book.RemoveAuthor(bookAuthor);

            await this._unitOfWork.CommitAsync(book, false);

            await this._eventBus.Publish(new AuthorRemovedFromBookIntegrationEvent(bookAuthor.AuthorGuid, book.Guid));

            if (book.Authors.Count == 0)
                await this._unknownAuthorService.AddBookToUnknownAuthorAsync(book.Guid);
        }
    }
}