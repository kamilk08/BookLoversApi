using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.IntegrationTests
{
    public class ChangeEmailTests : IntegrationTest<AuthModule, ChangeEmailCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private Guid _userGuid;
        private ChangeEmailWriteModel _changeEmailWriteModel;

        [Test]
        public async Task ChangeEmail_WhenCalled_ShouldChangeUserEmail()
        {
            var validation = await Module.SendCommandAsync(Command);
            validation.HasErrors.Should().BeFalse();

            var user = await Module.ExecuteQueryAsync<GetUserByGuidQuery, UserDto>(new GetUserByGuidQuery(_userGuid));

            user.Should().NotBeNull();
            user.Value.Email.Should().Be(_changeEmailWriteModel.NextEmail);
        }

        [Test]
        public async Task ChangeEmail_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validation = await Module.SendCommandAsync(Command);
            validation.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task ChangeEmail_WhenCalledAndCurrentEmailAndNextEmailAreInvalid_ShouldReturnFailureResult()
        {
            Command = new ChangeEmailCommand(new ChangeEmailWriteModel
            {
                NextEmail = string.Empty,
                Email = string.Empty
            });

            var validation = await Module.SendCommandAsync(Command);
            validation.HasErrors.Should().BeTrue();
            validation.Errors.Should().Contain(p => p.ErrorProperty == "NextEmail");
            validation.Errors.Should().Contain(p => p.ErrorProperty == "Email");
        }

        [Test]
        public async Task ChangeEmail_WhenCalledAndEmailIsInvalid_ShouldReturnValidationErrorWithErrors()
        {
            Command = new ChangeEmailCommand(new ChangeEmailWriteModel
            {
                Email = Fixture.Create<string>(),
                NextEmail = Fixture.Create<string>(),
            });

            var validation = await Module.SendCommandAsync(Command);
            validation.HasErrors.Should().BeTrue();
            validation.Errors.Should().Contain(p => p.ErrorProperty == "NextEmail");
            validation.Errors.Should().Contain(p => p.ErrorProperty == "Email");
        }

        [Test]
        public async Task ChangeEmail_WhenCalledAndAccountHasBeenBlocked_ShouldThrowBusinessRuleNotMeetException()
        {
            var user = await UnitOfWork.GetAsync<User>(_userGuid);

            user.BlockAccount();

            await UnitOfWork.CommitAsync(user);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        protected override void InitializeRoot()
        {
            _userGuid = Fixture.Create<Guid>();

            _httpContextAccessorMock.Setup(s => s.UserGuid).Returns(_userGuid);

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            _managerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            AuthModuleStartup.Initialize(
                _httpContextAccessorMock.Object,
                _managerMock.Object,
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
            var userFactorySetup = this.CompositionRoot.Kernel.Get<UserFactorySetup>();
            var signUpWriteModel = this.Fixture
                .Build<SignUpWriteModel>()
                .With(p => p.BookcaseGuid)
                .With(p => p.UserGuid, this._userGuid)
                .With(p => p.ProfileGuid)
                .With(p => p.Account, new AccountWriteModel
                {
                    AccountDetails = new AccountDetailsWriteModel
                    {
                        Email = "www.email1@gmail.com",
                        UserName = this.Fixture.Create<string>()
                    },
                    AccountSecurity = this.Fixture.Create<AccountSecurityWriteModel>()
                }).Create();

            var factoryData = UserFactoryData
                .Initialize()
                .WithBasics(signUpWriteModel.UserGuid, signUpWriteModel.Account.AccountDetails.UserName)
                .WithAccount(
                    signUpWriteModel.Account.AccountDetails.Email,
                    signUpWriteModel.Account.AccountSecurity.Password);

            var user = userFactorySetup.SetupFactory(factoryData).CreateUser();

            await this.UnitOfWork.CommitAsync(user);
            this._changeEmailWriteModel = this.Fixture
                .Build<ChangeEmailWriteModel>()
                .With(p => p.Email, "www.email1@gmail.com")
                .With(p => p.NextEmail, "www.email2@gmail.com")
                .Create();

            this.Command = new ChangeEmailCommand(this._changeEmailWriteModel);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}