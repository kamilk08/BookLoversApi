using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class RemovePublisherCycleHandler : ICommandHandler<RemovePublisherCycleInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public RemovePublisherCycleHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(RemovePublisherCycleInternalCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.PublisherGuid);

            var cycle = publisher.GetCycle(command.PublisherCycleGuid);

            publisher.RemoveCycle(cycle);

            await this._unitOfWork.CommitAsync(publisher, false);
        }
    }
}