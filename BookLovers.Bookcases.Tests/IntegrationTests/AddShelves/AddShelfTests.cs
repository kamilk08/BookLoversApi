using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
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

namespace BookLovers.Bookcases.Tests.IntegrationTests.AddShelves
{
    public class AddShelfTests : IntegrationTest<BookcaseModule, AddShelfCommand>
    {
        private readonly Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private int _bookcaseId;
        private string _shelfName;

        [Test]
        public async Task AddShelf_Should_CreateNewCustomShelf()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var shelf = await Module.ExecuteQueryAsync<ShelfByNameQuery, ShelfDto>(
                new ShelfByNameQuery(_bookcaseId, _shelfName));

            shelf.Should().NotBeNull();
            shelf.Value.ShelfName.Should().Be(Command.WriteModel.ShelfName);
        }

        [Test]
        public async Task AddShelf_WhenCommandIsNotValid_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().ContainSingle(p => p.GetType() == typeof(FluentValidationError));
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
            validationResult.Errors.First().ErrorProperty.Should().Be("command");
        }

        [Test]
        public async Task AddShelf_WhenSelectedShelfAlreadyExists_ShouldThrowBusinessRuleNotMeetException()
        {
            await Module.SendCommandAsync(Command);

            Func<Task> result = async () => await Module.SendCommandAsync(Command);

            result.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase cannot have duplicated shelves.");
        }

        [Test]
        public async Task AddShelf_WhenCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            var validationResult = await Module.SendCommandAsync(
                new AddShelfCommand(new AddShelfWriteModel
                {
                    BookcaseGuid = Guid.Empty,
                    ShelfGuid = Guid.Empty
                }));

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookcaseGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ShelfGuid");
        }

        [Test]
        public async Task AddShelf_WhenUserIsTryingToAddShelfWithInvalidLength_ShouldReturnValidationResultWithErrors()
        {
            Command.WriteModel.ShelfName = "FO";

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ShelfName");
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
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

            var bookcaseGuid = Fixture.Create<Guid>();
            var readerGuid = Fixture.Create<Guid>();
            var readerId = Fixture.Create<int>();

            var aggregate = bookcaseFactory.Create(bookcaseGuid, readerGuid, readerId);

            await UnitOfWork.CommitAsync(aggregate);

            _bookcaseId = CompositionRoot.Kernel.Get<BookcaseContext>().Bookcases.First().Id;

            _shelfName = Fixture.Create<string>();

            var writeModel = new AddShelfWriteModel()
            {
                BookcaseGuid = bookcaseGuid,
                ShelfGuid = Fixture.Create<Guid>(),
                ShelfName = _shelfName
            };

            Command = new AddShelfCommand(writeModel);
        }
    }
}