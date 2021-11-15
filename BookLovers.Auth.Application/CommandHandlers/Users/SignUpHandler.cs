using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class SignUpHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserFactorySetup _userFactorySetup;
        private readonly IReadContextAccessor _contextAccessor;
        private readonly IInMemoryEventBus _eventBus;

        public SignUpHandler(
            IUnitOfWork unitOfWork,
            UserFactorySetup userFactorySetup,
            IReadContextAccessor contextAccessor,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._userFactorySetup = userFactorySetup;
            this._contextAccessor = contextAccessor;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(SignUpCommand command)
        {
            var factoryData =
                UserFactoryData.Initialize()
                    .WithBasics(command.WriteModel.UserGuid, command.WriteModel.Account.AccountDetails.UserName)
                    .WithAccount(
                        command.WriteModel.Account.AccountDetails.Email,
                        command.WriteModel.Account.AccountSecurity.Password);

            var user = this._userFactorySetup.SetupFactory(factoryData).CreateUser();

            await this._unitOfWork.CommitAsync(user);

            var integrationEvent = UserSignedUpIntegrationEvent
                .Initialize()
                .WithReader(this._contextAccessor.GetReadModelId(user.Guid), command.WriteModel.UserGuid)
                .WithAccount(
                    user.UserName.Value,
                    user.Account.Email.Value,
                    user.Account.AccountDetails.AccountCreateDate)
                .WithBookcase(command.WriteModel.BookcaseGuid)
                .WithProfile(command.WriteModel.ProfileGuid).Build();

            await this._eventBus.Publish(integrationEvent);
        }
    }
}