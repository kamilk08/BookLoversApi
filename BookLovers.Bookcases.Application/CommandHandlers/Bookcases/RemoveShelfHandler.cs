using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class RemoveShelfHandler : ICommandHandler<RemoveShelfCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;

        public RemoveShelfHandler(IUnitOfWork unitOfWork, IInMemoryEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(RemoveShelfCommand command)
        {
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.ShelfWriteModel.BookcaseGuid);

            bookcase.RemoveShelf(bookcase.GetShelf(command.ShelfWriteModel.ShelfGuid));

            await _unitOfWork.CommitAsync(bookcase);

            await _eventBus.Publish(new ShelfRemovedIntegrationEvent(bookcase.Additions.ReaderGuid));
        }
    }
}