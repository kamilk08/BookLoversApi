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
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
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

namespace BookLovers.Bookcases.Tests.IntegrationTests.ChangeShelf
{
    public class ChangeShelfTestsWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, ChangeShelfCommand>
    {
        private Guid _userGuid;
        private Guid _bookcaseGuid;
        private int _bookcaseId;
        private List<Shelf> _shelves;

        [Test]
        public async Task ChangeShelf_WhenCalled_ShouldChangeBookShelf()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await Module.ExecuteQueryAsync<ShelfByGuidQuery, ShelfDto>(
                new ShelfByGuidQuery(Command.WriteModel.NewShelfGuid));

            queryResult.Value.Publications.Should().HaveCount(1);
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
            _shelves = new List<Shelf>();
            var bookcaseFactory = new BookcaseFactory();
            _bookcaseGuid = Fixture.Create<Guid>();
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, _userGuid, readerId);

            await UnitOfWork.CommitAsync(bookcase);
            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            var book = new BookcaseBook(Fixture.Create<Guid>(), Fixture.Create<Guid>(),
                Fixture.Create<int>());

            await UnitOfWork.CommitAsync(book);

            Enumerable.Range(0, 15).ToList().ForEach(i =>
            {
                var shelf = Shelf.CreateCustomShelf(Fixture.Create<Guid>(), Fixture.Create<string>());

                bookcase.AddCustomShelf(shelf);
                _shelves.Add(shelf);
            });

            await UnitOfWork.CommitAsync(bookcase);

            await Module.SendCommandAsync(new AddToBookcaseCommand(new AddBookToBookcaseWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid,
                ShelfGuid = _shelves.First().Guid
            }));

            for (var i = 0; i < _shelves.Count - 1; i++)
            {
                Command = new ChangeShelfCommand(new ChangeShelfWriteModel()
                {
                    BookcaseGuid = _bookcaseGuid,
                    OldShelfGuid = _shelves[i].Guid,
                    NewShelfGuid = _shelves[i + 1].Guid,
                    BookGuid = book.BookGuid
                });

                await Module.SendCommandAsync(Command);
            }

            Command = new ChangeShelfCommand(new ChangeShelfWriteModel
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid,
                NewShelfGuid = _shelves.First().Guid,
                OldShelfGuid = _shelves.Last().Guid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}