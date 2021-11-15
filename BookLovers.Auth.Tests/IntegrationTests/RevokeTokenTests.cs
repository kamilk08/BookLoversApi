using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
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
    public class RevokeTokenTests : IntegrationTest<AuthModule, RevokeTokenCommand>
    {
        private readonly Guid _audienceGuid = new Guid("e5fadbd2-c9e9-4408-950f-7800ff6d1c0c");
        private Guid _userGuid;
        private Guid _tokenGuid;

        [Test]
        public async Task RevokeToken_WhenCalled_ShouldRevokeToken()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<GetRefreshTokenReadModelQuery, RefreshTokenReadModel>(
                    new GetRefreshTokenReadModelQuery(_tokenGuid));
            queryResult.Value.RevokedAt.Should().NotBeNull();
        }

        [Test]
        public async Task RevokeToken_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task RevokeToken_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new RevokeTokenCommand(new RevokeTokenWriteModel
            {
                TokenGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Count().Should().Be(1);
        }

        protected override void InitializeRoot()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var appManagerMock = new Mock<IAppManager>();

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);


            AuthModuleStartup.Initialize(
                httpContextAccessor.Object,
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
            var userFactorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

            _userGuid = Fixture.Create<Guid>();

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

            var tokenProperties = new RefreshTokenProperties()
            {
                AudienceGuid = _audienceGuid,
                RefreshTokenLifeTime = 1440, // minutes
                Issuer = Fixture.Create<string>(),
                SerializedTicket = Fixture.Create<string>(),
                Email = user.Account.Email.Value,
                IssuedAt = DateTimeOffset.UtcNow,
                SigningKey = Fixture.Create<string>()
            };
            var command = new CreateRefreshTokenCommand(tokenProperties);
            await Module.SendCommandAsync(command);

            var revokeTokenDto = Fixture.Build<RevokeTokenWriteModel>()
                .With(p => p.TokenGuid, command.TokenProperties.TokenGuid)
                .Create<RevokeTokenWriteModel>();

            Command = new RevokeTokenCommand(revokeTokenDto);
            this._tokenGuid = command.TokenProperties.TokenGuid;
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}