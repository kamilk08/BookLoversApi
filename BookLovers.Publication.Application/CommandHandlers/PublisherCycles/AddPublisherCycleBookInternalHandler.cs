using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.PublisherCycles
{
    internal class AddPublisherCycleBookInternalHandler :
        ICommandHandler<AddPublisherCycleBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddPublisherCycleBookInternalHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddPublisherCycleBookInternalCommand command)
        {
            var cycle = await this._unitOfWork.GetAsync<PublisherCycle>(command.CycleGuid);

            cycle.AddBook(new CycleBook(command.BookGuid));

            await this._unitOfWork.CommitAsync(cycle);

            if (command.SendIntegrationEvent)
                await this._inMemoryEventBus.Publish(
                    new PublisherCycleHasNewBookIntegrationEvent(cycle.Guid, command.BookGuid));
        }
    }
}