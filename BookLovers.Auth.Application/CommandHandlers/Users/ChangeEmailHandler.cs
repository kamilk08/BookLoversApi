using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class ChangeEmailHandler : ICommandHandler<ChangeEmailCommand>
    {
        private readonly IInMemoryEventBus _eventBus;
        private readonly IUserRepository _repository;
        private readonly IEmailUniquenessChecker _uniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeEmailHandler(
            IInMemoryEventBus eventBus,
            IUserRepository repository,
            IEmailUniquenessChecker uniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            this._eventBus = eventBus;
            this._repository = repository;
            this._uniquenessChecker = uniquenessChecker;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeEmailCommand command)
        {
            var user = await this._repository.GetUserByEmailAsync(command.WriteModel.Email);

            user.ChangeEmail(command.WriteModel.NextEmail, this._uniquenessChecker);

            await this._unitOfWork.CommitAsync(user);

            var @event = new UserChangedEmailIntegrationEvent(user.Guid, command.WriteModel.NextEmail);

            await this._eventBus.Publish(@event);
        }
    }
}