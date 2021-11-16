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
using BookLovers.Bookcases.Domain.BookcaseBooks;
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

namespace BookLovers.Bookcases.Tests.IntegrationTests.RemoveFromBookcase
{
    public class RemoveFromBookcaseTestsWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, RemoveFromBookcaseCommand>
    {
        private Guid _userGuid;
        private Guid _bookcaseGuid;
        private int _bookcaseId;
        private int _readerId;
        private List<BookcaseBook> _books;

        [Test]
        public async Task RemoveFromBookcase_WhenCalled_ShouldRemoveBookFromBookcase()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<BookcaseByGuidQuery, BookcaseDto>(
                    new BookcaseByGuidQuery(_bookcaseGuid));

            queryResult.Value.BooksCount.Should().Be(0);
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
            _books = new List<BookcaseBook>();

            var bookcaseFactory = new BookcaseFactory();
            _bookcaseGuid = Fixture.Create<Guid>();
            _readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, _userGuid, _readerId);

            await UnitOfWork.CommitAsync(bookcase);
            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            var addShelfDto = new AddShelfWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = Fixture.Create<Guid>(),
                ShelfName = Fixture.Create<string>()
            };

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            await Enumerable.Range(0, 5).ForEach(async i =>
            {
                var book = new BookcaseBook(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<int>());
                await UnitOfWork.CommitAsync(book);

                _books.Add(book);
            });

            await Enumerable.Range(0, 5).ForEach(async i =>
            {
                await Module.SendCommandAsync(new AddToBookcaseCommand(new AddBookToBookcaseWriteModel()
                {
                    BookcaseGuid = _bookcaseGuid,
                    BookGuid = _books[i].BookGuid,
                    ShelfGuid = addShelfDto.ShelfGuid
                }));
            });

            await Enumerable.Range(0, 4).ForEach(async (i) => await Module.SendCommandAsync(
                new RemoveFromBookcaseCommand(
                    new RemoveFromBookcaseWriteModel()
                    {
                        BookcaseGuid = _bookcaseGuid,
                        BookGuid = _books[i].BookGuid
                    })));

            Command = new RemoveFromBookcaseCommand(new RemoveFromBookcaseWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = _books.Last().BookGuid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}