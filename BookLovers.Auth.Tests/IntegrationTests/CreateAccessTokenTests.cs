using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Application.WriteModels;
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
    public class CreateAccessTokenTests : IntegrationTest<AuthModule, CreateAccessTokenCommand>
    {
        private string _issuer;

        [Test]
        public async Task CreateAccessToken_WhenCalled_ShouldCreateProtectedToken()
        {
            await this.Module.SendCommandAsync(Command);

            var tokenManager = CompositionRoot.Kernel.Get<ITokenManager>();
            var protectedToken = await tokenManager.GetTokenAsync(Command.TokenGuid);

            protectedToken.Should().NotBeEmpty();
        }

        [Test]
        public async Task CreateAccessToken_WhenCalledAndSigningKeyIsNull_ShouldReturnValidationResultWithError()
        {
            Command.Dto.SigningKey = string.Empty;

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().ContainSingle(p => p.ErrorProperty == "SigningKey");
        }

        [Test]
        public async Task CreateAccessToken_WhenCalledAndAudienceIsInvalid_ShouldReturnValidationResultWithErrors()
        {
            Command.Dto.AudienceId = string.Empty;

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().ContainSingle(p => p.ErrorProperty == "AudienceId");
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
            var addAudienceDto = Fixture.Build<AddAudienceWriteModel>()
                .With(p => p.AudienceGuid)
                .With(p => p.TokenLifeTime)
                .Without(p => p.AudienceSecretKey)
                .Create<AddAudienceWriteModel>();

            var addAudienceCommand = new AddAudienceCommand(addAudienceDto);

            await Module.SendCommandAsync(addAudienceCommand);

            _issuer = Fixture.Create<string>();

            var dto = new AccessTokenProperties
            {
                AudienceId = addAudienceDto.AudienceGuid.ToString(),
                Claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Email, "testemail@gmail.com") },
                Issuer = _issuer,
                SigningKey = addAudienceDto.AudienceSecretKey,
                IssuedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
            };

            Command = new CreateAccessTokenCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}