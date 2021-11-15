using System;
using System.Configuration;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Audiences;
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
    public class AddAudienceTests : IntegrationTest<AuthModule, AddAudienceCommand>
    {
        [Test]
        public async Task AddAudience_WhenCalled_ShouldAddNewAudience()
        {
            await this.Module.SendCommandAsync(Command);

            var audience =
                await Module.ExecuteQueryAsync<GetAudienceQuery, AudienceReadModel>(
                    new GetAudienceQuery(Command.WriteModel.AudienceGuid.ToString()));

            audience.Should().NotBeNull();
        }

        [Test]
        public async Task AddAudience_WhenCalledAndCommandIsInValid_ShouldReturnFailureResult()
        {
            Command = new AddAudienceCommand(new AddAudienceWriteModel
            {
                AudienceGuid = Guid.Empty,
                AudienceSecretKey = string.Empty,
                TokenLifeTime = 0
            });

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task AddAudience_WhenCalledAndCommandIsNull_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
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

        protected override Task PrepareData()
        {
            var writeModel = Fixture.Build<AddAudienceWriteModel>()
                .With(p => p.TokenLifeTime, 1440)
                .With(p => p.AudienceGuid).Without(p => p.AudienceSecretKey).Create();

            Command = new AddAudienceCommand(writeModel);

            return Task.CompletedTask;
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}