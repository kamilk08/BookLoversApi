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
    public class ChangePasswordTests : IntegrationTest<AuthModule, ChangePasswordCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private Guid _userGuid;

        [Test]
        public async Task ChangePassword_WhenCalled_ShouldChangeUserPassword()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var user = await Module.ExecuteQueryAsync<GetUserByGuidQuery, UserDto>(new GetUserByGuidQuery(_userGuid));

            user.Should().NotBeNull();
        }

        [Test]
        public async Task ChangePassword_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task ChangePassword_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new ChangePasswordCommand(new ChangePasswordWriteModel
            {
                NewPassword = string.Empty,
                Password = string.Empty
            });

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "NewPassword");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Password");
        }

        public async Task ChangePassword_WhenCalledAndPasswordHasInvalidLength_ShouldReturnFailureResult()
        {
            Command = new ChangePasswordCommand(new ChangePasswordWriteModel
            {
                NewPassword = "ABC"
            });

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "NewPassword");
        }

        [Test]
        public async Task ChangePassword_WhenCalledAndUserIsNotActive_ShouldReturnBusinessRuleNotMeetException()
        {
            var user = await this.UnitOfWork.GetAsync<User>(_userGuid);
            user.ArchiveAggregate();

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
            var userFactorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

            var password = "Abc123456789X";

            var signUpDto = Fixture.Build<SignUpWriteModel>()
                .With(p => p.BookcaseGuid)
                .With(p => p.UserGuid, _userGuid)
                .With(p => p.ProfileGuid)
                .With(p => p.Account, new AccountWriteModel
                {
                    AccountDetails = new AccountDetailsWriteModel
                    {
                        Email = "www.email1@gmail.com",
                        UserName = Fixture.Create<string>()
                    },
                    AccountSecurity = new AccountSecurityWriteModel
                    {
                        Password = password
                    }
                })
                .Create<SignUpWriteModel>();

            var userFactoryData = UserFactoryData.Initialize()
                .WithBasics(signUpDto.UserGuid, signUpDto.Account.AccountDetails.UserName)
                .WithAccount(signUpDto.Account.AccountDetails.Email, signUpDto.Account.AccountSecurity.Password);

            var userFactory = userFactorySetup.SetupFactory(userFactoryData);

            var user = userFactory.CreateUser();

            await UnitOfWork.CommitAsync(user);

            var changePasswordDto = Fixture.Build<ChangePasswordWriteModel>()
                .With(p => p.Password, password)
                .With(p => p.NewPassword, password + password)
                .With(p => p.Email, signUpDto.Account.AccountDetails.Email)
                .Create<ChangePasswordWriteModel>();

            Command = new ChangePasswordCommand(changePasswordDto);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}