using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.CommandHandlers
{
    internal class CreateLibrarianHandler : ICommandHandler<CreateLibrarianCommand>
    {
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly IPromotionWaiterRepository _repository;

        public CreateLibrarianHandler(
            IInMemoryEventBus inMemoryEventBus,
            IUnitOfWork unitOfWork,
            IReadContextAccessor readContextAccessor,
            IPromotionWaiterRepository repository)
        {
            this._inMemoryEventBus = inMemoryEventBus;
            this._unitOfWork = unitOfWork;
            this._readContextAccessor = readContextAccessor;
            this._repository = repository;
        }

        public async Task HandleAsync(CreateLibrarianCommand command)
        {
            var librarian = new Librarian(command.WriteModel.LibrarianGuid, command.WriteModel.ReaderGuid);
            var promotionWaiter = await this._repository.GetPromotionWaiterByReaderGuid(command.WriteModel.ReaderGuid);

            if (promotionWaiter == null)
                throw new BusinessRuleNotMetException("Selected user does not exist.");

            await this._unitOfWork.CommitAsync(librarian);

            var @event = new ReaderPromotedToLibrarian(command.WriteModel.ReaderGuid, command.WriteModel.LibrarianGuid);

            await this._inMemoryEventBus.Publish(@event);

            command.WriteModel.LibrarianId = this._readContextAccessor.GetReadModelId(librarian.Guid);
        }
    }
}