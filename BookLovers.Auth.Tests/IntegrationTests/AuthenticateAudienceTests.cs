using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Auth.Domain.Users.Services;
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
    public class AuthenticateAudienceTests :
        IntegrationTest<AuthModule, AuthenticateAudienceCommand>
    {
        private Guid _audienceGuid;
        private string _secretKey;

        [Test]
        public async Task AuthenticateAudience_WhenCalledAndAudienceCredentialsAreValid_ShouldAuthenticateProperly()
        {
            var command = new AuthenticateAudienceCommand(_audienceGuid.ToString(), _secretKey);

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeTrue();
        }

        [Test]
        public async Task
            AuthenticateAudience_WhenCalledAndAudienceCredentialsAreInValid_ShouldNotAuthenticateAudience()
        {
            var command = new AuthenticateAudienceCommand(_audienceGuid.ToString(), Fixture.Create<string>());

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeFalse();
        }

        [Test]
        public async Task AuthenticateAudience_WhenCalledAndAudienceIsNotActive_ShouldNotAuthenticateAudience()
        {
            var audienceRepository = CompositionRoot.Kernel.Get<IRepository<Audience>>();
            var audience = await audienceRepository.GetAsync(_audienceGuid);
            audience.Block();
            await audienceRepository.CommitChangesAsync(audience);

            var command = new AuthenticateAudienceCommand(_audienceGuid.ToString(), _secretKey);

            await this.Module.SendCommandAsync(command);

            command.IsAuthenticated.Should().BeFalse();
        }

        [Test]
        public async Task
            AuthenticateAudience_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new AuthenticateAudienceCommand(string.Empty, string.Empty);

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
            var unitOfWork = CompositionRoot.Kernel.Get<IUnitOfWork>();

            _audienceGuid = Fixture.Create<Guid>();
            _secretKey = Fixture.Create<string>();

            var hashingService = CompositionRoot.Kernel.Get<IHashingService>();
            var hashWithSalt = hashingService.CreateHashWithSalt(_secretKey);

            var security = new AudienceSecurity(hashWithSalt.Item1, hashWithSalt.Item2);
            var details = new AudienceDetails(AudienceType.AngularSpa, 144000);

            var audience = new Audience(_audienceGuid, security, details);

            await unitOfWork.CommitAsync(audience);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}