using System;
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
    internal class CreateSuperAdminHandler : ICommandHandler<CreateSuperAdminCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SuperAdminFactorySetup _factorySetup;
        private readonly IReadContextAccessor _contextAccessor;
        private readonly IInMemoryEventBus _eventBus;

        public CreateSuperAdminHandler(
            IUnitOfWork unitOfWork,
            SuperAdminFactorySetup factorySetup,
            IReadContextAccessor contextAccessor,
            IInMemoryEventBus eventBus)
        {
            this._unitOfWork = unitOfWork;
            this._factorySetup = factorySetup;
            this._contextAccessor = contextAccessor;
            this._eventBus = eventBus;
        }

        public async Task HandleAsync(CreateSuperAdminCommand command)
        {
            var factoryData = UserFactoryData.Initialize()
                .WithBasics(command.SignUpWriteModel.UserGuid, command.SignUpWriteModel.Account.AccountDetails.UserName)
                .WithAccount(
                    command.SignUpWriteModel.Account.AccountDetails.Email,
                    command.SignUpWriteModel.Account.AccountSecurity.Password);

            var user = this._factorySetup.SetupFactory(factoryData).CreateUser();
            user.ConfirmAccount(DateTime.UtcNow);

            await this._unitOfWork.CommitAsync(user, false);

            var integrationEvent = SuperAdminCreatedIntegrationEvent
                .Initialize()
                .WithReader(
                    this._contextAccessor.GetReadModelId(user.Guid),
                    command.SignUpWriteModel.UserGuid)
                .WithAccount(
                    user.UserName.Value,
                    user.Account.Email.Value,
                    user.Account.AccountDetails.AccountCreateDate)
                .WithBookcase(command.SignUpWriteModel.BookcaseGuid)
                .WithProfile(command.SignUpWriteModel.ProfileGuid)
                .Build();

            await this._eventBus.Publish(integrationEvent);
        }
    }
}