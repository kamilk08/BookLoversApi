using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
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

namespace BookLovers.Publication.Tests.IntegrationTests.AddPublisherCycleBook
{
    public class AddPublisherCycleBookWithEventStoreSnapshot :
        IntegrationTest<PublicationModule, AddPublisherCycleBookCommand>
    {
        private Guid _readerGuid;
        private string _cycleName;
        private PublisherCycle _publisherCycle;

        [Test]
        public async Task AddPublisherCycleBook_WhenCalled_ShouldAddBookToPublisherCycle()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await Module.ExecuteQueryAsync<CycleByNameQuery, PublisherCycleDto>(
                new CycleByNameQuery(_publisherCycle.CycleName));

            queryResult.Should().NotBeNull();
            queryResult.Value.CycleBooks.Should().HaveCount(1);
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
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<PublicationsStoreContext>().ClearPublicationsStore();
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<PublicationsContext>().CleanPublicationsContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var bookReaderId = Fixture.Create<int>();

            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, bookReaderId);

            await UnitOfWork.CommitAsync(bookReader);

            var bookFactory = CompositionRoot.Kernel.Get<BookFactory>();

            var publisher = new Publisher(Fixture.Create<Guid>(), Fixture.Create<string>());

            await UnitOfWork.CommitAsync(publisher);

            var authorDto = Fixture.Build<AuthorWriteModel>()
                .WithBooks(new List<Guid>())
                .WithBasics(Sex.Male, Fixture.Create<string>(), Fixture.Create<string>())
                .WithDescription(Fixture.Create<string>(), Fixture.Create<string>(), Fixture.Create<string>())
                .WithDetails(Fixture.Create<DateTime>(), Fixture.Create<string>(), Fixture.Create<DateTime>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Thriller.Value })
                .With(p => p.AuthorGuid)
                .With(p => p.ReaderGuid, _readerGuid)
                .Create<AuthorWriteModel>();

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();
            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            var data = this.CreateBookFactoryData(author.Guid, publisher.Guid);

            var book = await bookFactory.CreateBook(data);

            await UnitOfWork.CommitAsync(book);

            _cycleName = Fixture.Create<string>();

            _publisherCycle = new PublisherCycle(Fixture.Create<Guid>(), publisher.Guid, _cycleName);

            await UnitOfWork.CommitAsync(_publisherCycle);

            await Enumerable.Range(0, 30).ForEach(async i =>
            {
                await Module.SendCommandAsync(new AddPublisherCycleBookCommand(_publisherCycle.Guid, book.Guid));

                await Module.SendCommandAsync(new RemovePublisherCycleBookCommand(book.Guid, _publisherCycle.Guid));
            });

            Command = new AddPublisherCycleBookCommand(_publisherCycle.Guid, book.Guid);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
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