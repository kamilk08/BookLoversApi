using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class AddAuthorBookHandler : ICommandHandler<AddAuthorBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddAuthorBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddAuthorBookInternalCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);

            author.AddBook(new AuthorBook(command.BookGuid));

            await this._unitOfWork.CommitAsync(author, false);
            if (!command.SendIntegrationEvent)
                return;

            await this._inMemoryEventBus.Publish(
                new AuthorHasNewBookIntegrationEvent(command.AuthorGuid, command.BookGuid));
        }
    }
}