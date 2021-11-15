using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class AddShelfHandler : ICommandHandler<AddShelfCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IReadContextAccessor _readContextAccessor;

        public AddShelfHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            IReadContextAccessor readContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _readContextAccessor = readContextAccessor;
        }

        public async Task HandleAsync(AddShelfCommand command)
        {
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);

            bookcase.AddCustomShelf(command.WriteModel.ShelfGuid, command.WriteModel.ShelfName);

            await _unitOfWork.CommitAsync(bookcase);

            command.WriteModel.ShelfId = _readContextAccessor.GetReadModelId(command.WriteModel.ShelfGuid);

            var @event = new ShelfCreatedIntegrationEvent(bookcase.Additions.ReaderGuid);

            await _eventBus.Publish(@event);
        }
    }
}