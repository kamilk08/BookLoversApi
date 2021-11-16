using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.ArchivePublisherCycle
{
    public class ArchivePublisherCycleTests :
        IntegrationTest<PublicationModule, ArchivePublisherCycleCommand>
    {
        private Guid _readerGuid;
        private Guid _publisherCycleGuid;

        [Test]
        public async Task ArchivePublisher_WhenCalled_ShouldArchivePublisherCycle()
        {
            var queryResult =
                await Module.ExecuteQueryAsync<CycleByGuidQuery, PublisherCycleDto>(
                    new CycleByGuidQuery(_publisherCycleGuid));

            queryResult.Value.Should().NotBeNull();

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            queryResult =
                await Module.ExecuteQueryAsync<CycleByGuidQuery, PublisherCycleDto>(
                    new CycleByGuidQuery(_publisherCycleGuid));

            queryResult.Value.Should().BeNull();
        }

        [Test]
        public async Task ArchivePublisher_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            var validationResult = await Module.SendCommandAsync(new ArchivePublisherCycleCommand(Guid.Empty));
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "PublisherCycleGuid");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            var appManagerMock = new Mock<IAppManager>();

            var publicationsConnectionString =
                Environment.GetEnvironmentVariable(PublicationsContext.ConnectionStringKey);
            if (publicationsConnectionString.IsEmpty())
                publicationsConnectionString = E2EConstants.PublicationsConnectionString;

            var publicationsStoreConnectionString =
                Environment.GetEnvironmentVariable(PublicationsStoreContext.ConnectionStringKey);
            if (publicationsStoreConnectionString.IsEmpty())
                publicationsStoreConnectionString = E2EConstants.PublicationsStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsContext.ConnectionStringKey))
                .Returns(publicationsConnectionString);
            appManagerMock.Setup(s => s.GetConfigValue(PublicationsStoreContext.ConnectionStringKey)).Returns(
                publicationsStoreConnectionString);

            PublicationModuleStartup.Initialize(new FakeHttpContextAccessor(_readerGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger());
        }

        protected override Task ClearStore()
        {
            this.CompositionRoot.Kernel.Get<PublicationsStoreContext>().ClearPublicationsStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            this.CompositionRoot.Kernel.Get<PublicationsContext>().CleanPublicationsContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, Fixture.Create<int>());
            await UnitOfWork.CommitAsync(bookReader);

            this._publisherCycleGuid = Fixture.Create<Guid>();

            var publisher = new Publisher(Fixture.Create<Guid>(), Fixture.Create<string>());

            await UnitOfWork.CommitAsync(publisher);

            var createPublisherCycle = new AddPublisherCycleCommand(new AddCycleWriteModel()
            {
                Cycle = Fixture.Create<string>(),
                CycleGuid = _publisherCycleGuid,
                PublisherGuid = publisher.Guid
            });

            await this.Module.SendCommandAsync(createPublisherCycle);

            Command = new ArchivePublisherCycleCommand(_publisherCycleGuid);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}