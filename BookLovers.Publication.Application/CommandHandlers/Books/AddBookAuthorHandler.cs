using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class AddBookAuthorHandler : ICommandHandler<AddBookAuthorInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddBookAuthorHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddBookAuthorInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.AddAuthor(new BookAuthor(command.AuthorGuid));

            await this._unitOfWork.CommitAsync(book, false);

            await this._inMemoryEventBus.Publish(
                new AuthorAddedToBookIntegrationEvent(command.AuthorGuid, command.BookGuid));
        }
    }
}