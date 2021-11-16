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
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.IntegrationTests.RemoveShelf
{
    public class RemoveShelfTestsWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, RemoveShelfCommand>
    {
        private Guid _bookcaseGuid;
        private Guid _userGuid;
        private List<Shelf> _shelves;

        [Test]
        public async Task RemoveShelf_WhenCalled_ShouldRemoveShelf()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<BookcaseByGuidQuery, BookcaseDto>(
                    new BookcaseByGuidQuery(_bookcaseGuid));

            // Three beceause every bookcase has 3 core shelves that are not removable
            queryResult.Value.ShelvesCount.Should().Be(3);
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

            BookcaseModuleStartup.Initialize(new FakeHttpContextAccessor(_userGuid, true), mock.Object,
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
            _shelves = new List<Shelf>();

            var bookcaseFactory = new BookcaseFactory();
            _bookcaseGuid = Fixture.Create<Guid>();
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, _userGuid, readerId);

            await UnitOfWork.CommitAsync(bookcase);

            Enumerable.Range(0, 30).ForEach(i =>
            {
                var shelf = Shelf.CreateCustomShelf(Fixture.Create<Guid>(), Fixture.Create<string>());

                bookcase.AddCustomShelf(shelf);

                _shelves.Add(shelf);
            });

            await UnitOfWork.CommitAsync(bookcase);

            await Enumerable.Range(0, 29).ForEach(async i =>
            {
                var removeShelfCommand = new RemoveShelfCommand(new RemoveShelfWriteModel()
                {
                    BookcaseGuid = _bookcaseGuid,
                    ShelfGuid = _shelves[i].Guid
                });

                await Module.SendCommandAsync(removeShelfCommand);
            });

            Command = new RemoveShelfCommand(new RemoveShelfWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = _shelves.Last().Guid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}