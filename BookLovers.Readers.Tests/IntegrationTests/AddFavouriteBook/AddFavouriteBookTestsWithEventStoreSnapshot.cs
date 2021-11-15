using System;
using System.Collections.Generic;
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
using BookLovers.Readers.Domain.Favourites;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.AddFavouriteBook
{
    public class AddFavouriteBookTestsWithEventStoreSnapshot :
        IntegrationTest<ReadersModule, AddFavouriteBookCommand>
    {
        private Guid _readerGuid;
        private Guid _bookGuid;
        private int _readerId;

        [Test]
        public async Task AddFavouriteBook_WhenCalled_ShouldAddNewFavouriteBook()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await this.Module.ExecuteQueryAsync<FavouriteBooksQuery, IEnumerable<FavouriteBookDto>>(
                    new FavouriteBooksQuery(_readerId));

            var favouriteAuthor = queryResult.Value.FirstOrDefault();

            favouriteAuthor.BookGuid.Should().Be(_bookGuid);
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

            var identification = new ReaderIdentification(_readerId, email, userName);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, statisticsGathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            _bookGuid = Fixture.Create<Guid>();
            var bookId = Fixture.Create<int>();
            var date = Fixture.Create<DateTime>();

            var resourceAdder = new BookResourceAdder();
            resourceAdder.AddResource(reader, new AddedBook(_bookGuid, bookId, date));

            await UnitOfWork.CommitAsync(reader);

            await UnitOfWork.CommitAsync(new Favourite(_bookGuid, Fixture.Create<int>(), _readerGuid));

            await Enumerable.Range(0, 30).ForEach(async i =>
            {
                await Module.SendCommandAsync(
                    new AddFavouriteBookCommand(new AddFavouriteBookWriteModel()
                    {
                        BookGuid = _bookGuid,
                        ProfileGuid = reader.Socials.ProfileGuid,
                        AddedAt = Fixture.Create<DateTime>()
                    }));
                await Module.SendCommandAsync(
                    new RemoveFavouriteCommand(new RemoveFavouriteWriteModel
                    {
                        FavouriteGuid = _bookGuid,
                        ProfileGuid = reader.Socials.ProfileGuid
                    }));
            });

            var dto = new AddFavouriteBookWriteModel()
            {
                BookGuid = _bookGuid,
                ProfileGuid = reader.Socials.ProfileGuid,
                AddedAt = Fixture.Create<DateTime>()
            };

            this.Command = new AddFavouriteBookCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}