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
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.IntegrationTests.AddToBookcase
{
    [TestFixture]
    public class AddToBookcaseTestsWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, AddToBookcaseCommand>
    {
        private int _bookcaseId;
        private int _readerId;
        private Guid _bookcaseGuid;
        private Guid _readerGuid;
        private List<Shelf> _shelves = new List<Shelf>();
        private List<BookcaseBook> _bookcaseBooks = new List<BookcaseBook>();
        private List<AddToBookcaseCommand> _commands = new List<AddToBookcaseCommand>();

        [Test]
        public async Task AddToBookcase_WhenCalled_ShouldAddBookToBookcase()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var bookcaseDto =
                await Module.ExecuteQueryAsync<BookcaseByGuidQuery, BookcaseDto>(
                    new BookcaseByGuidQuery(_bookcaseGuid));

            bookcaseDto.Value.BooksCount.Should().Be(20);
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
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

            BookcaseModuleStartup.Initialize(new FakeHttpContextAccessor(_readerGuid, true), appManagerMock.Object,
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
            _readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, _readerGuid, _readerId);
            await UnitOfWork.CommitAsync(bookcase);
            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            Enumerable.Range(0, 20).ForEach(i =>
            {
                var shelf = Shelf.CreateCustomShelf(Fixture.Create<Guid>(), Fixture.Create<string>());
                _shelves.Add(shelf);
                bookcase.AddCustomShelf(shelf);
            });

            await Enumerable.Range(0, 20).ForEach(async i =>
            {
                var bookcaseBook = new BookcaseBook(
                    Fixture.Create<Guid>(),
                    Fixture.Create<Guid>(),
                    Fixture.Create<int>());

                await UnitOfWork.CommitAsync(bookcaseBook);
                _bookcaseBooks.Add(bookcaseBook);
            });

            _bookcaseBooks.Take(19).ToList().ForEach((book, i) =>
            {
                var @event = BookAddedToShelf.Initialize()
                    .WithBookcase(bookcase.Guid)
                    .WithReader(_readerGuid)
                    .WithBookAndShelf(book.BookGuid, _shelves[i].Guid)
                    .WithAddedAt(DateTime.UtcNow)
                    .WithTracker(bookcase.Additions.ShelfRecordTrackerGuid);

                bookcase.ApplyChange(@event);
            });

            await UnitOfWork.CommitAsync(bookcase);

            Command = new AddToBookcaseCommand(new AddBookToBookcaseWriteModel
            {
                BookcaseGuid = bookcase.Guid,
                BookGuid = _bookcaseBooks.Last().BookGuid,
                ShelfGuid = _shelves.Last().Guid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}