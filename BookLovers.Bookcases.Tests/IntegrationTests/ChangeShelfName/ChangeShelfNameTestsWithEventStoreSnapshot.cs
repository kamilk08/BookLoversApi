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

namespace BookLovers.Bookcases.Tests.IntegrationTests.ChangeShelfName
{
    public class ChangeShelfNameTestsWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, ChangeShelfNameCommand>
    {
        private Guid _userGuid;
        private Guid _bookcaseGuid;
        private int _bookcaseId;
        private string _shelfName;
        private Guid _shelfGuid;

        [Test]
        public async Task ChangeShelfName_WhenCalled_NameShelfShouldBeChanged()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await Module.ExecuteQueryAsync<ShelfByNameQuery, ShelfDto>(
                new ShelfByNameQuery(_bookcaseId, _shelfName));

            queryResult.Value.ShelfName.Should().Be(Command.WriteModel.ShelfName);
        }

        protected override void InitializeRoot()
        {
            _userGuid = Fixture.Create<Guid>();

            var appManagerMock = new Mock<IAppManager>();

            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

            appManagerMock
                .Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            appManagerMock
                .Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            BookcaseModuleStartup.Initialize(new FakeHttpContextAccessor(_userGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
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
            var bookcaseFactory = new BookcaseFactory();
            _bookcaseGuid = Fixture.Create<Guid>();
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, _userGuid, readerId);
            await UnitOfWork.CommitAsync(bookcase);

            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;
            _shelfGuid = Fixture.Create<Guid>();
            _shelfName = Fixture.Create<string>();

            var addShelfDto = new AddShelfWriteModel
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = _shelfGuid,
                ShelfName = _shelfName
            };

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            await Enumerable.Range(0, 15).ForEach(async (i) => await Module.SendCommandAsync(
                new ChangeShelfNameCommand(new ChangeShelfNameWriteModel
                {
                    BookcaseGuid = _bookcaseGuid,
                    ShelfGuid = _shelfGuid,
                    ShelfName = Fixture.Create<string>()
                })));

            var dto = new ChangeShelfNameWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = _shelfGuid,
                ShelfName = _shelfName
            };

            Command = new ChangeShelfNameCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}