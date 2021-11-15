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
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.IntegrationTests.ChangeShelf
{
    [TestFixture]
    public class ChangeShelfTests : IntegrationTest<BookcaseModule, ChangeShelfCommand>
    {
        private readonly Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private int _bookcaseId;
        private Guid _bookcaseGuid;
        private string _firstShelfName;
        private string _secondShelfName;
        private Guid _firstShelfGuid;
        private Guid _secondShelfGuid;

        [Test]
        public async Task ChangeShelf_WhenCalled_ShouldChangeBookShelf()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var shelf =
                await Module.ExecuteQueryAsync<ShelfByGuidQuery, ShelfDto>(
                    new ShelfByGuidQuery(_secondShelfGuid));

            shelf.Value.Publications.Should().HaveCount(1);
        }

        [Test]
        public async Task ChangeShelf_WhenCalledAndCommandIsNull_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task ChangeShelf_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWitheErrors()
        {
            Command = new ChangeShelfCommand(new ChangeShelfWriteModel()
            {
                NewShelfGuid = Guid.Empty,
                OldShelfGuid = Guid.Empty,
                BookcaseGuid = Guid.Empty,
                BookGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();

            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "NewShelfGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "OldShelfGuid");
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
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, readerGuid, readerId);

            await UnitOfWork.CommitAsync(bookcase);
            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            var book = new BookcaseBook(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<int>());

            await UnitOfWork.CommitAsync(book);

            _firstShelfName = Fixture.Create<string>();
            _firstShelfGuid = Fixture.Create<Guid>();
            _secondShelfName = Fixture.Create<string>();
            _secondShelfGuid = Fixture.Create<Guid>();

            var addShelfDto = new AddShelfWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = _firstShelfGuid,
                ShelfName = _firstShelfName
            };

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            addShelfDto = new AddShelfWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                ShelfGuid = _secondShelfGuid,
                ShelfName = _secondShelfName
            };

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            await Module.SendCommandAsync(new AddToBookcaseCommand(new AddBookToBookcaseWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid,
                ShelfGuid = _firstShelfGuid
            }));

            Command = new ChangeShelfCommand(new ChangeShelfWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                BookGuid = book.BookGuid,
                NewShelfGuid = _secondShelfGuid,
                OldShelfGuid = _firstShelfGuid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}