using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.IntegrationTests
{
    [TestFixture]
    public class AuthenticateUserTests : IntegrationTest<AuthModule, AuthenticateUserCommand>
    {
        private string _email;
        private string _password;

        [Test]
        public async Task AuthenticateUser_WhenCalled_ShouldAuthenticateUser()
        {
            var command = new AuthenticateUserCommand(_email, _password);

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeTrue();
        }

        [Test]
        public async Task
            AuthenticateUser_WhenCalledAndUserTriesToAuthenticateWithInvalidCredentials_ShouldNotAuthenticateUser()
        {
            var command = new AuthenticateUserCommand(_email, Fixture.Create<string>());

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeFalse();
        }

        [Test]
        public async Task AuthenticateUser_WhenCalledAndUserIsSuperAdmin_ShouldAuthenticateUser()
        {
            var superAdminCommand = CreateSuperAdminCommand.Create();

            await this.Module.SendCommandAsync(superAdminCommand);

            var command = new AuthenticateUserCommand(
                superAdminCommand.SignUpWriteModel.Account.AccountDetails.Email,
                superAdminCommand.SignUpWriteModel.Account.AccountSecurity.Password);

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeTrue();
        }

        [Test]
        public async Task AuthenticateUser_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new AuthenticateUserCommand(string.Empty, string.Empty);

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        protected override void InitializeRoot()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            var appManagerMock = new Mock<IAppManager>();

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            AuthModuleStartup.Initialize(
                httpContextAccessorMock.Object,
                appManagerMock.Object,
                new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<AuthContext>().CleanAuthContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            this.Module = CompositionRoot.Kernel.Get<IValidationDecorator<AuthModule>>();

            _email = Fixture.Create<string>();
            _password = Fixture.Create<string>();

            var unitOfWork = CompositionRoot.Kernel.Get<IUnitOfWork>();
            var factorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

            var userFactoryData = UserFactoryData.Initialize()
                .WithAccount(_email, _password)
                .WithBasics(Fixture.Create<Guid>(), Fixture.Create<string>());

            var user = factorySetup.SetupFactory(userFactoryData).CreateUser();
            user.ConfirmAccount(DateTime.UtcNow);

            await unitOfWork.CommitAsync(user);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}