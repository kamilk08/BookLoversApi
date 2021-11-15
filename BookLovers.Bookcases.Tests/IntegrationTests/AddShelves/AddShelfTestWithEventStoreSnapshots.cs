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
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.IntegrationTests.AddShelves
{
    public class AddShelfTestWithEventStoreSnapshots : IntegrationTest<BookcaseModule, AddShelfCommand>
    {
        private int _bookcaseId;
        private Guid _userGuid;
        private string _shelfName;
        private int _snapshottingFrequency = 10;
        private int _maxSnapshottingFrequency = 100;

        [Test]
        public async Task AddShelf_WhenCalledAndAggregateIsCreatedFromSnapshot_ShouldCreateNewCustomShelf()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var shelf = await Module.ExecuteQueryAsync<ShelfByNameQuery, ShelfDto>(
                new ShelfByNameQuery(_bookcaseId, _shelfName));

            shelf.Should().NotBeNull();
            shelf.Value.ShelfName.Should().Be(Command.WriteModel.ShelfName);
        }

        protected override void InitializeRoot()
        {
            _userGuid = Fixture.Create<Guid>();
            var mock = new Mock<IAppManager>();

            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

            mock
                .Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            mock
                .Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            BookcaseModuleStartup
                .Initialize(new FakeHttpContextAccessor(_userGuid, true), mock.Object,
                    new FakeLogger().GetLogger(),
                    new PersistenceSettings(
                        new SnapshotSettings(
                            _snapshottingFrequency,
                            new SnapshotConstraints(_snapshottingFrequency, _maxSnapshottingFrequency)),
                        PersistenceInitialSettings.Default()));
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<BookcaseStoreContext>().ClearBookcaseStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<BookcaseContext>().CleanBookcaseContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var bookcaseGuid = Fixture.Create<Guid>();
            var bookcaseFactory = new BookcaseFactory();
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(bookcaseGuid, _userGuid, readerId);

            await UnitOfWork.CommitAsync(bookcase);

            Enumerable.Range(0, 12)
                .ForEach(i => bookcase.AddCustomShelf(Fixture.Create<Guid>(), Fixture.Create<string>()));

            await UnitOfWork.CommitAsync(bookcase);

            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            _shelfName = Fixture.Create<string>();

            Command = new AddShelfCommand(new AddShelfWriteModel
            {
                BookcaseGuid = bookcaseGuid,
                ShelfGuid = Fixture.Create<Guid>(),
                ShelfName = _shelfName
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}