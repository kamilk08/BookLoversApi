using System;
using System.Linq;
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
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.FollowReader
{
    public class FollowReaderTests : IntegrationTest<ReadersModule, FollowReaderCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpAccessorMock = new Mock<IHttpContextAccessor>();
        private Guid _followedGuid;
        private int _readerId;
        private Guid _readerGuid;
        private int _followedId;

        [Test]
        public async Task FollowReader_WhenCalled_ShouldAddNewFollower()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await this.Module.ExecuteQueryAsync<ReaderPaginatedFollowersQuery, PaginatedResult<int>>(
                    new ReaderPaginatedFollowersQuery(_followedId, string.Empty, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage));

            queryResult.Value.TotalItems.Should().Be(1);
            queryResult.Value.Items.First().Should().Be(_readerId);
        }

        [Test]
        public async Task FollowReader_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();

            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task FollowReader_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new FollowReaderCommand(new ReaderFollowWriteModel
            {
                FollowedGuid = Guid.Empty
            });

            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "FollowedGuid");
        }

        [Test]
        public async Task FollowReader_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var reader = await UnitOfWork.GetAsync<Reader>(Command.Dto.FollowedGuid);
            reader.ApplyChange(new ReaderSuspended(reader.Guid, reader.Socials.ProfileGuid,
                reader.Socials.NotificationWallGuid));

            await UnitOfWork.CommitAsync(reader);

            Func<Task> act = async () => await this.Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void FollowReader_WhenCalledAndReaderTriesToFollowHimself_ShouldThrowBusinessRuleNotMeetException()
        {
            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_followedGuid);

            Func<Task> act = async () => await this.Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot follow himself.");
        }

        [Test]
        public async Task
            FollowReader_WhenCalledAndReaderAlreadyHasTheSameFollower_ShouldThrowBusinessRuleNotMeetException()
        {
            await this.Module.SendCommandAsync(Command);

            Func<Task> act = async () => await this.Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot have duplicated followers.");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_readerGuid);

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
            
            ReadersModuleStartup.Initialize(_httpAccessorMock.Object, appManagerMock.Object,
                new FakeLogger().GetLogger(),
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
            var gathererGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();
            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, gathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            var followedEmail = this.Fixture.Create<string>();
            _followedId = Fixture.Create<int>();
            var followed = readerFactory.Create(
                _followedGuid,
                new ReaderIdentification(_followedId, Fixture.Create<string>(), followedEmail),
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>()));

            await UnitOfWork.CommitAsync(reader);

            await UnitOfWork.CommitAsync(followed);

            var dto = new ReaderFollowWriteModel
            {
                FollowedGuid = _followedGuid
            };

            this.Command = new FollowReaderCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}