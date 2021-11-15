using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class RemovePublisherBookHandler : ICommandHandler<RemovePublisherBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public RemovePublisherBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(RemovePublisherBookInternalCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.PublisherGuid);

            publisher.RemoveBook(publisher.GetBook(command.BookGuid));

            await this._unitOfWork.CommitAsync(publisher, false);

            await this._inMemoryEventBus.Publish(
                new PublisherLostBookIntegrationEvent(publisher.Guid, command.BookGuid));
        }
    }
}