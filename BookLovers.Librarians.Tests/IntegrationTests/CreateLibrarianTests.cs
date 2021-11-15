using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using BookLovers.Librarians.Infrastructure.Root;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.IntegrationTests
{
    public class CreateLibrarianTests : IntegrationTest<LibrarianModule, CreateLibrarianCommand>
    {
        private Guid _userGuid;
        private Guid _librarianGuid;

        [Test]
        public async Task CreateLibrarian_WhenCalled_ShouldReturnValidationResultWithNoErrorsAndCreateLibrarian()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var librarian =
                await Module.ExecuteQueryAsync<LibrarianByGuidQuery, LibrarianDto>(
                    new LibrarianByGuidQuery(_librarianGuid));

            librarian.Should().NotBeNull();
        }

        [Test]
        public async Task CreateLibrarian_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            this.Command = new CreateLibrarianCommand(new CreateLibrarianWriteModel
            {
                LibrarianGuid = Guid.Empty,
                ReaderGuid = Guid.Empty
            });

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "LibrarianGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ReaderGuid");
        }

        protected override void InitializeRoot()
        {
            _userGuid = Fixture.Create<Guid>();

            var appManagerMock = new Mock<IAppManager>();

            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;
            
            appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);


            LibrarianModuleStartup.Initialize(new FakeHttpContextAccessor(_userGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<LibrariansContext>().CleanLibrarianContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var promotionWaiter = new PromotionWaiter(Fixture.Create<Guid>(), _userGuid, Fixture.Create<int>());

            await UnitOfWork.CommitAsync(promotionWaiter);

            _librarianGuid = Fixture.Create<Guid>();

            Command = new CreateLibrarianCommand(new CreateLibrarianWriteModel()
            {
                LibrarianGuid = _librarianGuid,
                ReaderGuid = _userGuid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}