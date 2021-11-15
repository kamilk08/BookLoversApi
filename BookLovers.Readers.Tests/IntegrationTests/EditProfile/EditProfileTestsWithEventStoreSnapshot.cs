using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Application.WriteModels.Profiles;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.EditProfile
{
    public class EditProfileTestsWithEventStoreSnapshot :
        IntegrationTest<ReadersModule, EditProfileCommand>
    {
        private Guid _readerGuid;
        private ProfileWriteModel _profileWriteModel;
        private int _readerId;

        [Test]
        public async Task EditProfile_WhenCalled_ShouldEditProfile()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await this.Module.ExecuteQueryAsync<ReaderProfileQuery, ProfileDto>(
                    new ReaderProfileQuery(_readerId));

            queryResult.Value.About.Should().Be(_profileWriteModel.About.Content);
            queryResult.Value.City.Should().Be(_profileWriteModel.Address.City);
            queryResult.Value.Country.Should().Be(_profileWriteModel.Address.Country);
            queryResult.Value.SexName.Should().Be(Sex.Male.Name);
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

            _readerId = this.Fixture.Create<int>();
            var profileGuid = this.Fixture.Create<Guid>();
            var notificationWallGuid = this.Fixture.Create<Guid>();
            var statisticsGathererGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, statisticsGathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            await Enumerable.Range(0, 30).ForEach(async (i) =>
            {
                var profileDto = _profileWriteModel = GetProfileDto(profileGuid);

                await this.Module.SendCommandAsync(new EditProfileCommand(profileDto));
            });

            this._profileWriteModel = GetProfileDto(profileGuid);

            this.Command = new EditProfileCommand(_profileWriteModel);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }

        private ProfileWriteModel GetProfileDto(Guid profileGuid)
        {
            return Fixture.Build<ProfileWriteModel>()
                .With(p => p.ProfileGuid, profileGuid)
                .With(p => p.About)
                .With(p => p.Address)
                .With(p => p.Identity, new IdentityWriteModel
                {
                    BirthDate = Fixture.Create<DateTime>(),
                    FirstName = Fixture.Create<string>(),
                    SecondName = Fixture.Create<string>(),
                    Sex = Sex.Male.Value
                })
                .Create();
        }
    }
}