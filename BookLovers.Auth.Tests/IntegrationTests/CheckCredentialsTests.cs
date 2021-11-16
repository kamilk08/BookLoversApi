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
    public class CheckCredentialsTests : IntegrationTest<AuthModule, CheckCredentialsCommand>
    {
        private string _username;
        private string _password;

        [Test]
        public async Task CheckCredentials_WhenCalledWithValidCredentials_ShouldAuthenticateUser()
        {
            var command = new CheckCredentialsCommand(_username, _password);

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeTrue();
        }

        [Test]
        public async Task CheckCredentials_WhenCalledWithInValidCredentials_ShouldNotAuthenticateUser()
        {
            var command = new CheckCredentialsCommand(_username, Fixture.Create<string>());

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeFalse();
        }

        protected override void InitializeRoot()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var appManagerMock = new Mock<IAppManager>();

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

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

            _username = Fixture.Create<string>();
            _password = Fixture.Create<string>();

            var unitOfWork = CompositionRoot.Kernel.Get<IUnitOfWork>();
            var factorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

            var userFactoryData = UserFactoryData.Initialize()
                .WithAccount(Fixture.Create<string>(), _password)
                .WithBasics(Fixture.Create<Guid>(), _username);

            var user = factorySetup.SetupFactory(userFactoryData).CreateUser();

            await unitOfWork.CommitAsync(user);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}