using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
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
    [TestFixture]
    public class RemoveFromBookcaseTests : IntegrationTest<BookcaseModule, RemoveFromBookcaseCommand>
    {
        private readonly Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private int _bookcaseId;
        private int _readerId;
        private string _shelfName;
        private Guid _bookcaseGuid;

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

        [Test]
        public async Task RemoveFromBookcase_WhenCalledAndCommandIsNull_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task RemoveFromBookcase_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new RemoveFromBookcaseCommand(new RemoveFromBookcaseWriteModel
            {
                BookcaseGuid = Guid.Empty,
                BookGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookcaseGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
        }

        protected override void InitializeRoot()
        {
            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

            _managerMock
                .Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            _managerMock
                .Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            BookcaseModuleStartup.Initialize(_mock.Object, _managerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
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
            var readerGuid = Fixture.Create<Guid>();
            _readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, readerGuid, _readerId);

            await UnitOfWork.CommitAsync(bookcase);
            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            _shelfName = Fixture.Create<string>();

            var addShelfDto = new AddShelfWriteModel
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = Fixture.Create<Guid>(),
                ShelfName = _shelfName
            };

            var book = new BookcaseBook(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<int>());

            await UnitOfWork.CommitAsync(book);

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            var addToBookcaseCommand = new AddToBookcaseCommand(new AddBookToBookcaseWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid,
                ShelfGuid = addShelfDto.ShelfGuid
            });

            await Module.SendCommandAsync(addToBookcaseCommand);

            Command = new RemoveFromBookcaseCommand(new RemoveFromBookcaseWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}