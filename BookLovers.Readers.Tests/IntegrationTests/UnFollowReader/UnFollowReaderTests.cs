using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Application.WriteModels.Readers;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.UnFollowReader
{
    public class UnFollowReaderTests : IntegrationTest<ReadersModule, UnFollowReaderCommand>
    {
        private Guid _readerGuid;
        private int _readerId;
        private Guid _followedGuid;
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private int _followedId;

        [Test]
        public async Task UnFollowReader_WhenCalled_ShouldRemoveReaderFollower()
        {
            await this.Module.SendCommandAsync(new FollowReaderCommand(new ReaderFollowWriteModel
            {
                FollowedGuid = _followedGuid
            }));

            var queryResult =
                await this.Module.ExecuteQueryAsync<ReaderPaginatedFollowersQuery, PaginatedResult<int>>(
                    new ReaderPaginatedFollowersQuery(_followedId, string.Empty, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage));

            queryResult.Value.TotalItems.Should().Be(1);

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            queryResult = await this.Module.ExecuteQueryAsync<ReaderPaginatedFollowersQuery, PaginatedResult<int>>(
                new ReaderPaginatedFollowersQuery(_followedId, string.Empty, PaginatedResult.DefaultPage,
                    PaginatedResult.DefaultItemsPerPage));

            queryResult.Value.TotalItems.Should().Be(0);
        }

        [Test]
        public async Task UnFollowReader_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = null;

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task UnFollowReader_WhenCalledAndCommandDataIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = new UnFollowReaderCommand(new ReaderFollowWriteModel
            {
                FollowedGuid = Guid.Empty
            });

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "FollowedGuid");
        }

        [Test]
        public void
            UnFollowReader_WhenCalledAndFollowedReaderDoesNotHaveSelectedFollower_ShouldThrowBusinessRuleNotMeetException()
        {
            _mock.Setup(s => s.UserGuid).Returns(Fixture.Create<Guid>());

            Func<Task> act = async () => await this.Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader does not have selected follower.");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _mock.Setup(p => p.UserGuid).Returns(_readerGuid);

            var appManagerMock = new Mock<IAppManager>();
            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;

            var readersStoreConnectionString =
                Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
            if (readersStoreConnectionString.IsEmpty())
                readersStoreConnectionString = E2EConstants.ReadersStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(ReadersContext.ConnectionStringKey))
                .Returns(readersConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(ReadersStoreContext.ConnectionStringKey))
                .Returns(readersStoreConnectionString);

            ReadersModuleStartup.Initialize(_mock.Object, appManagerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<ReadersStoreContext>().CleanReadersStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<ReadersContext>().CleanReadersContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var readerFactory = new ReaderFactory();

            _followedGuid = Fixture.Create<Guid>();

            _readerId = this.Fixture.Create<int>();
            var profileGuid = this.Fixture.Create<Guid>();
            var notificationWallGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, Fixture.Create<Guid>());

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            var followedEmail = Fixture.Create<string>();
            _followedId = Fixture.Create<int>();
            var followed = readerFactory.Create(
                _followedGuid,
                new ReaderIdentification(_followedId, Fixture.Create<string>(), followedEmail),
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>()));

            await UnitOfWork.CommitAsync(reader);

            await UnitOfWork.CommitAsync(followed);

            var followDto = new ReaderFollowWriteModel
            {
                FollowedGuid = _followedGuid
            };

            Command = new UnFollowReaderCommand(followDto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}