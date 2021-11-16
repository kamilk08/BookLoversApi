using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
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

namespace BookLovers.Publication.Tests.IntegrationTests
{
    public class CreatePublisherCycleTests :
        IntegrationTest<PublicationModule, AddPublisherCycleCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private AddCycleWriteModel _addCycleWriteModel;

        [Test]
        public async Task CreatePublisherCycle_WhenCalled_ShouldAddNewPublisherCycle()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<CycleByNameQuery, PublisherCycleDto>(
                    new CycleByNameQuery(_addCycleWriteModel.Cycle));

            queryResult.Should().NotBeNull();
        }

        [Test]
        public async Task CreatePublisherCycle_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task CreatePublisherCycle_WhenCalledAndCommandHasNotValidData_ShouldReturnFailureResult()
        {
            Command = new AddPublisherCycleCommand(new AddCycleWriteModel
            {
                CycleGuid = Guid.Empty,
                Cycle = string.Empty,
                PublisherGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "CycleGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Cycle");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "PublisherGuid");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _mock.Setup(s => s.UserGuid).Returns(_readerGuid);

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

            PublicationModuleStartup.Initialize(_mock.Object, appManagerMock.Object, new FakeLogger().GetLogger());
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

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();
            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            var bookData = this.CreateBookFactoryData(author.Guid, publisher.Guid);

            var book = await bookFactory.CreateBook(bookData);

            await UnitOfWork.CommitAsync(book);

            _addCycleWriteModel = Fixture.Build<AddCycleWriteModel>()
                .With(p => p.Cycle)
                .With(p => p.CycleGuid)
                .With(p => p.PublisherGuid, publisher.Guid)
                .Create<AddCycleWriteModel>();

            Command = new AddPublisherCycleCommand(_addCycleWriteModel);
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