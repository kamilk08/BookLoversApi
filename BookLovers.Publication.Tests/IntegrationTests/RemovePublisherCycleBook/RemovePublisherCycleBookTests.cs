using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.RemovePublisherCycleBook
{
    public class RemovePublisherCycleBookTests :
        IntegrationTest<PublicationModule, RemovePublisherCycleBookCommand>
    {
        private Guid _readerGuid;
        private Guid _publisherGuid;
        private Guid _publisherCycleGuid;
        private string _cycleName;

        [Test]
        public async Task RemovePublisherCycleBook_WhenCalled_ShouldRemoveBookAndReturnValidationResultWithoutErrors()
        {
            var queryResult =
                await this.Module.ExecuteQueryAsync<CycleByGuidQuery, PublisherCycleDto>(
                    new CycleByGuidQuery(_publisherCycleGuid));

            queryResult.Value.CycleBooks.Should().HaveCount(1);

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            queryResult =
                await this.Module.ExecuteQueryAsync<CycleByGuidQuery, PublisherCycleDto>(
                    new CycleByGuidQuery(_publisherCycleGuid));

            queryResult.Value.CycleBooks.Should().HaveCount(0);
        }

        [Test]
        public async Task
            RemovePublisherCycleBook_WhenCalledAndCommandHasInvalidData_ReturnValidationResultWithoutErrors()
        {
            Command = new RemovePublisherCycleBookCommand(Guid.Empty, Guid.Empty);

            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();

            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "CycleGuid");
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
            var bookReaderId = Fixture.Create<int>();

            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, bookReaderId);

            await UnitOfWork.CommitAsync(bookReader);

            var bookFactory = CompositionRoot.Kernel.Get<BookFactory>();

            _publisherGuid = Fixture.Create<Guid>();

            var publisher = new Publisher(_publisherGuid, Fixture.Create<string>());

            await UnitOfWork.CommitAsync(publisher);

            var authorDto = Fixture.Build<AuthorWriteModel>()
                .WithBooks(new List<Guid>())
                .WithBasics(Sex.Male, Fixture.Create<string>(), Fixture.Create<string>())
                .WithDescription(Fixture.Create<string>(), Fixture.Create<string>(), Fixture.Create<string>())
                .WithDetails(Fixture.Create<DateTime>(), Fixture.Create<string>(), Fixture.Create<DateTime>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Thriller.Value })
                .With(p => p.AuthorGuid)
                .With(p => p.ReaderGuid, _readerGuid)
                .Create();

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();
            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            var data = this.CreateBookFactoryData(author.Guid, publisher.Guid);

            var book = await bookFactory.CreateBook(data);

            await UnitOfWork.CommitAsync(book);

            _cycleName = Fixture.Create<string>();
            _publisherCycleGuid = Fixture.Create<Guid>();

            var cycle = new PublisherCycle(_publisherCycleGuid, publisher.Guid, _cycleName);

            await UnitOfWork.CommitAsync(cycle);

            var addPublisherCycleBookCommand = new AddPublisherCycleBookCommand(cycle.Guid, book.Guid);

            await this.Module.SendCommandAsync(addPublisherCycleBookCommand);

            this.Command = new RemovePublisherCycleBookCommand(book.Guid, _publisherCycleGuid);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }

        private AuthorData CreateAuthorFactoryData()
        {
            return AuthorData.Initialize()
                .WithBasics(
                    new FullName(Fixture.Create<string>(), Fixture.Create<string>()),
                    Sex.Male)
                .WithDetails(
                    new LifeLength(Fixture.Create<DateTime>(), Fixture.Create<DateTime>()),
                    Fixture.Create<string>())
                .WithDescription(
                    Fixture.Create<string>(),
                    Fixture.Create<string>(), Fixture.Create<string>())
                .WithGuid(Fixture.Create<Guid>())
                .AddedBy(new BookReader(Fixture.Create<Guid>(), _readerGuid, Fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Thriller.Value });
        }

        private BookData CreateBookFactoryData(Guid authorGuid, Guid publisherGuid)
        {
            var bookBasics = BookBasicsData.Initialize()
                .WithTitle(Fixture.Create<string>())
                .WithIsbn("9788375155280")
                .WithDate(Fixture.Create<DateTime>())
                .WithCategory(new BookCategory(Category.Fiction, SubCategory.FictionSubCategory.Fantasy));

            return BookData.Initialize(Fixture.Create<Guid>(), new List<Guid> { authorGuid })
                .WithBasics(bookBasics, publisherGuid)
                .WithDetails(new BookDetailsData(Fixture.Create<int>(), Language.English))
                .WithDescription(new BookDescriptionData(Fixture.Create<string>(), Fixture.Create<string>()))
                .WithSeries(new BookSeriesData(Guid.Empty, default(byte)))
                .WithCycles(new List<Guid>())
                .AddedBy(this._readerGuid)
                .WithCover(new BookCoverData(CoverType.PaperBack, Fixture.Create<string>()))
                .WithHashTags(Fixture.Create<List<string>>());
        }
    }
}