using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Persistence;
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
    [TestFixture]
    public class CreateRefreshTokenTests : IntegrationTest<AuthModule, CreateRefreshTokenCommand>
    {
        private Guid _userGuid;

        [Test]
        public async Task CreateRefreshToken_WhenCalled_ShouldCreateRefreshToken()
        {
            var tokenManager = CompositionRoot.Kernel.Get<ITokenManager>();

            var result = await this.Module.SendCommandAsync(Command);

            var refreshToken = await tokenManager.GetTokenAsync(Command.TokenProperties.TokenGuid);

            refreshToken.Should().NotBeEmpty();
            result.HasErrors.Should().BeFalse();
        }

        [Test]
        public async Task CreateRefreshToken_WhenCalledAndCommandIsInvalid_ShouldReturnValidationFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
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
            _userGuid = Fixture.Create<Guid>();

            var userFactorySetup = CompositionRoot.Kernel.Get<UserFactorySetup>();

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

            var userFactoryData = UserFactoryData.Initialize()
                .WithBasics(signUpDto.UserGuid, signUpDto.Account.AccountDetails.UserName)
                .WithAccount(signUpDto.Account.AccountDetails.Email, signUpDto.Account.AccountSecurity.Password);

            var userFactory = userFactorySetup.SetupFactory(userFactoryData);

            var user = userFactory.CreateUser();

            await UnitOfWork.CommitAsync(user);

            var addAudienceDto = new AddAudienceWriteModel
            {
                AudienceGuid = Guid.NewGuid(),
                TokenLifeTime = 1440
            };

            var createAudienceCommand = new AddAudienceCommand(addAudienceDto);
            await Module.SendCommandAsync(createAudienceCommand);

            var tokenProperties = new RefreshTokenProperties()
            {
                AudienceGuid = addAudienceDto.AudienceGuid,
                Email = user.Account.Email.Value,
                IssuedAt = DateTimeOffset.UtcNow,
                Issuer = Fixture.Create<string>(),
                RefreshTokenLifeTime = 1440,
                SerializedTicket = Fixture.Create<string>(),
                SigningKey = createAudienceCommand.WriteModel.AudienceSecretKey
            };

            Command = new CreateRefreshTokenCommand(tokenProperties);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}