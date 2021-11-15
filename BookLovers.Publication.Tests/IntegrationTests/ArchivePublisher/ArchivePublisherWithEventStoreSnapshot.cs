using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Application.WriteModels.Publisher;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.ArchivePublisher
{
    public class ArchivePublisherWithEventStoreSnapshot :
        IntegrationTest<PublicationModule, ArchivePublisherCommand>
    {
        private Guid _readerGuid;
        private Guid _publisherGuid;
        private string _publisherName;

        [Test]
        public async Task ArchivePublisher_WhenCalled_ShouldReturnValidationResultWithNoErrors()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<PublisherByNameQuery, PublisherDto>(
                    new PublisherByNameQuery(_publisherName));

            queryResult.Value.Should().BeNull();
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

            await UnitOfWork.CommitAsync(new Publisher(Guid.NewGuid(), SelfPublisher.Key));

            _publisherGuid = Fixture.Create<Guid>();
            _publisherName = Fixture.Create<string>();

            var createPublisherCommand = new CreatePublisherCommand(new AddPublisherWriteModel
            {
                Name = _publisherName,
                PublisherGuid = _publisherGuid,
            });
            await this.Module.SendCommandAsync(createPublisherCommand);

            await Enumerable.Range(0, 30).ForEach(async i =>
            {
                await Module.SendCommandAsync(new AddPublisherCycleCommand(new AddCycleWriteModel
                {
                    PublisherGuid = _publisherGuid,
                    CycleGuid = Fixture.Create<Guid>(),
                    Cycle = Fixture.Create<string>()
                }));
            });

            Command = new ArchivePublisherCommand(_publisherGuid);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}