using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Registrations;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Registrations;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.IntegrationTests
{
    public class CompleteRegistrationTests : IntegrationTest<AuthModule, CompleteRegistrationCommand>
    {
        private Guid _userGuid;
        private string _registrationToken;

        [Test]
        public async Task CompleteRegistration_WhenCalled_ShouldCompleteRegistration()
        {
            var result = await Module.SendCommandAsync(Command);

            result.HasErrors.Should().BeFalse();

            var registration =
                await Module.ExecuteQueryAsync<GetRegistrationSummaryByUserGuidQuery, RegistrationSummaryReadModel>(
                    new GetRegistrationSummaryByUserGuidQuery(_userGuid));

            registration.Should().NotBeNull();
            registration.Value.CompletedAt.Should().NotBeNull();
        }

        [Test]
        public async Task CompleteRegistration_WhenCalledAndCommandIsInvalid_ShouldReturnValidationFailureResult()
        {
            Command = null;

            var result = await Module.SendCommandAsync(Command);

            result.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task CompleteRegistration_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationWithErrors()
        {
            Command = new CompleteRegistrationCommand(string.Empty, string.Empty);

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Email");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Token");
        }

        protected override void InitializeRoot()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var appManagerMock = new Mock<IAppManager>();
            var logger = new FakeLogger().GetLogger();

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            AuthModuleStartup.Initialize(
                httpContextAccessorMock.Object,
                appManagerMock.Object,
                logger,
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
            _userGuid = Fixture.Create<Guid>();
            _registrationToken = Fixture.Create<string>();

            var userFactorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

            var signUpDto = Fixture.Build<SignUpWriteModel>()
                .With(p => p.BookcaseGuid)
                .With(p => p.UserGuid, _userGuid)
                .With(p => p.ProfileGuid)
                .With(p => p.Account, new AccountWriteModel
                {
                    AccountDetails = new AccountDetailsWriteModel()
                    {
                        Email = "www.email1@gmail.com",
                        UserName = Fixture.Create<string>()
                    },
                    AccountSecurity = Fixture.Create<AccountSecurityWriteModel>()
                })
                .Create<SignUpWriteModel>();

            var userFactoryData = UserFactoryData
                .Initialize()
                .WithBasics(signUpDto.UserGuid, signUpDto.Account.AccountDetails.UserName)
                .WithAccount(signUpDto.Account.AccountDetails.Email, signUpDto.Account.AccountSecurity.Password);

            var userFactory = userFactorySetup.SetupFactory(userFactoryData);

            var user = userFactory.CreateUser();

            await UnitOfWork.CommitAsync(user);

            var registrationReadModel =
                await Module.ExecuteQueryAsync<GetRegistrationSummaryByUserGuidQuery, RegistrationSummaryReadModel>(
                    new GetRegistrationSummaryByUserGuidQuery(user.Guid));

            _registrationToken = registrationReadModel.Value.Token;

            Command = new CompleteRegistrationCommand(
                user.Account.Email.Value,
                _registrationToken);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}