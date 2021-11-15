using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class BlockAccountHandler : ICommandHandler<BlockAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly AccountBlocker _accountBlocker;

        public BlockAccountHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus inMemoryEventBus,
            AccountBlocker accountBlocker)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
            this._accountBlocker = accountBlocker;
        }

        public async Task HandleAsync(BlockAccountCommand command)
        {
            var user = await this._unitOfWork.GetAsync<User>(command.WriteModel.BlockedReaderGuid);

            this._accountBlocker.BlockUser(user);

            await this._unitOfWork.CommitAsync(user);

            var @event = new UserBlockedIntegrationEvent(user.Guid, user.IsInRole(Role.Librarian.Name));

            await this._inMemoryEventBus.Publish(@event);
        }
    }
}