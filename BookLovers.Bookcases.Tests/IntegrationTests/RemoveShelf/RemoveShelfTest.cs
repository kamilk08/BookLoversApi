using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Validation;
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

namespace BookLovers.Bookcases.Tests.IntegrationTests.RemoveShelf
{
    public class RemoveShelfTest : IntegrationTest<BookcaseModule, RemoveShelfCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();

        private Guid _shelfGuid;
        private string _shelfName;
        private int _bookcaseId;

        [Test]
        public async Task RemoveShelf_WhenCalled_ShouldRemoveShelf()
        {
            var addShelfDto = new AddShelfWriteModel()
            {
                BookcaseGuid = Command.ShelfWriteModel.BookcaseGuid,
                ShelfGuid = Command.ShelfWriteModel.ShelfGuid,
                ShelfName = _shelfName
            };

            await Module.SendCommandAsync(new AddShelfCommand(addShelfDto));

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await Module.ExecuteQueryAsync<ShelfByNameQuery, ShelfDto>(
                new ShelfByNameQuery(_bookcaseId, _shelfName));

            queryResult.Value.Should().BeNull();
        }

        [Test]
        public async Task RemoveShelf_WhenCalledAndDataInDtoIsNotValid_ShouldReturnFailureResult()
        {
            var validationResult = await Module.SendCommandAsync(new RemoveShelfCommand(
                new RemoveShelfWriteModel()
                {
                    BookcaseGuid = Guid.Empty,
                    ShelfGuid = Guid.Empty
                }));

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookcaseGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ShelfGuid");
        }

        [Test]
        public async Task RemoveShelf_WhenCalledAndCommandIsInvalid_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().ContainSingle(p => p.GetType() == typeof(FluentValidationError));
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
            validationResult.Errors.First().ErrorProperty.Should().Be("command");
        }

        [Test]
        public void RemoveShelf_WhenCalledAndThereIsNoSelectedShelf_ShouldThrowBusinessRuleNotMeetException()
        {
            Module = CompositionRoot.Kernel.Get<IValidationDecorator<BookcaseModule>>();

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
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

            BookcaseModuleStartup.Initialize(_httpContextAccessorMock.Object, _managerMock.Object,
                new FakeLogger().GetLogger(),
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
            var bookcaseGuid = Fixture.Create<Guid>();
            var readerGuid = Fixture.Create<Guid>();
            var readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(bookcaseGuid, readerGuid, readerId);

            await UnitOfWork.CommitAsync(bookcase);

            _shelfGuid = Fixture.Create<Guid>();
            _shelfName = Fixture.Create<string>();

            var dto = new RemoveShelfWriteModel()
            {
                BookcaseGuid = bookcaseGuid,
                ShelfGuid = _shelfGuid
            };

            Command = new RemoveShelfCommand(dto);

            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}