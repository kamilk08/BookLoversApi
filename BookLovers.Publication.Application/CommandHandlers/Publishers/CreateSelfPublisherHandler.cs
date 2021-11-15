using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class CreateSelfPublisherHandler : ICommandHandler<CreateSelfPublisherCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly IReadContextAccessor _readContextAccessor;

        public CreateSelfPublisherHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus inMemoryEventBus,
            IReadContextAccessor readContextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
            this._readContextAccessor = readContextAccessor;
        }

        public async Task HandleAsync(CreateSelfPublisherCommand command)
        {
            var publisher = new Publisher(command.PublisherGuid, SelfPublisher.Key);

            await this._unitOfWork.CommitAsync(publisher);

            await this._inMemoryEventBus.Publish(new PublisherCreatedIntegrationEvent(
                publisher.Guid,
                this._readContextAccessor.GetReadModelId(publisher.Guid)));
        }
    }
}