using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.PublisherCycles
{
    internal class RemovePublisherCycleBookHandler : ICommandHandler<RemovePublisherCycleBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public RemovePublisherCycleBookHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(RemovePublisherCycleBookCommand command)
        {
            var publisherCycle = await this._unitOfWork.GetAsync<PublisherCycle>(command.CycleGuid);

            var cycleBook = publisherCycle.GetCycleBook(command.BookGuid);

            publisherCycle.RemoveBook(cycleBook);

            await this._unitOfWork.CommitAsync(publisherCycle);

            await this._inMemoryEventBus.Publish(
                new BookRemovedFromPublisherCycleIntegrationEvent(command.CycleGuid, command.BookGuid));
        }
    }
}