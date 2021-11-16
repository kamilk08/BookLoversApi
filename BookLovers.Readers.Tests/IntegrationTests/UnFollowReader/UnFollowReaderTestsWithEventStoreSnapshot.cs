using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
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
    public class UnFollowReaderTestsWithEventStoreSnapshot :
        IntegrationTest<ReadersModule, UnFollowReaderCommand>
    {
        private Guid _readerGuid;
        private int _readerId;
        private Guid _followedGuid;
        private int _followedId;

        [Test]
        public async Task UnFollowReader_WhenCalled_ShouldRemoveReaderFollower()
        {
            await this.Module.SendCommandAsync(new FollowReaderCommand(new ReaderFollowWriteModel
            {
                FollowedGuid = _followedGuid
            }));

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await this.Module.ExecuteQueryAsync<ReaderPaginatedFollowersQuery, PaginatedResult<int>>(
                    new ReaderPaginatedFollowersQuery(_followedId, string.Empty, 0, 10));

            queryResult.Value.TotalItems.Should().Be(0);
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();

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

            ReadersModuleStartup.Initialize(new FakeHttpContextAccessor(_readerGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
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
            await UnitOfWork.CommitAsync(reader);

            var followedEmail = Fixture.Create<string>();
            _followedId = Fixture.Create<int>();
            var followed = readerFactory.Create(
                _followedGuid,
                new ReaderIdentification(_followedId, Fixture.Create<string>(), followedEmail),
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>()));

            await UnitOfWork.CommitAsync(followed);

            var followDto = new ReaderFollowWriteModel
            {
                FollowedGuid = _followedGuid
            };

            await Enumerable.Range(0, 30).ForEach(async (i) =>
            {
                await this.Module.SendCommandAsync(new FollowReaderCommand(new ReaderFollowWriteModel
                {
                    FollowedGuid = _followedGuid
                }));

                await this.Module.SendCommandAsync(new UnFollowReaderCommand(new ReaderFollowWriteModel
                {
                    FollowedGuid = _followedGuid
                }));
            });

            Command = new UnFollowReaderCommand(followDto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}