using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.PublisherCycles
{
    internal class AddPublisherCycleHandler : ICommandHandler<AddPublisherCycleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublisherCycleFactory _factory;
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly IReadContextAccessor _contextAccessor;

        public AddPublisherCycleHandler(
            IUnitOfWork unitOfWork,
            PublisherCycleFactory factory,
            IInMemoryEventBus inMemoryEventBus,
            IReadContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._factory = factory;
            this._inMemoryEventBus = inMemoryEventBus;
            this._contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(AddPublisherCycleCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.WriteModel.PublisherGuid);
            var cycle = this._factory.Create(command.WriteModel.CycleGuid, publisher, command.WriteModel.Cycle);

            await this._unitOfWork.CommitAsync(cycle);

            command.WriteModel.PublisherCycleId = this._contextAccessor.GetReadModelId(cycle.Guid);

            await this._inMemoryEventBus.Publish(new PublisherCycleCreatedIntegrationEvent(
                cycle.Guid,
                command.WriteModel.PublisherCycleId, command.WriteModel.PublisherGuid));
        }
    }
}