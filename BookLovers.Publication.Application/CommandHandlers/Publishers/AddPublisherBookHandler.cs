using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class AddPublisherBookHandler : ICommandHandler<AddPublisherBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddPublisherBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddPublisherBookInternalCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.PublisherGuid);

            publisher.AddBook(new PublisherBook(command.BookGuid));

            await this._unitOfWork.CommitAsync(publisher, false);

            if (command.SendIntegrationEvent)
                await this._inMemoryEventBus.Publish(
                    new PublisherHasNewBookIntegrationEvent(publisher.Guid, command.BookGuid));
        }
    }
}