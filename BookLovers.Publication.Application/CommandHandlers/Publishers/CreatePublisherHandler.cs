using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Publishers.Services;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Publishers
{
    internal class CreatePublisherHandler : ICommandHandler<CreatePublisherCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IReadContextAccessor _contextAccessor;
        private readonly PublisherFactory _factory;

        public CreatePublisherHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            IReadContextAccessor contextAccessor,
            PublisherFactory factory)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._contextAccessor = contextAccessor;
            this._factory = factory;
        }

        public async Task HandleAsync(CreatePublisherCommand command)
        {
            var publisher = this._factory.Create(command.WriteModel.PublisherGuid, command.WriteModel.Name);

            await this._unitOfWork.CommitAsync(publisher);

            command.WriteModel.PublisherId = this._contextAccessor.GetReadModelId(publisher.Guid);

            await this._eventBus.Publish(
                new PublisherCreatedIntegrationEvent(publisher.Guid, command.WriteModel.PublisherId));
        }
    }
}