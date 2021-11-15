using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class RemoveAuthorBookHandler : ICommandHandler<RemoveAuthorBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;

        public RemoveAuthorBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(RemoveAuthorBookInternalCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);
            var authorBook = author.GetBook(command.BookGuid);

            author.RemoveBook(authorBook);

            await this._unitOfWork.CommitAsync(author, false);

            await this._eventBus.Publish(new AuthorLostBookIntegrationEvent(author.Guid, authorBook.BookGuid));
        }
    }
}