using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Books;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.EditBook
{
    public class BookEditTests : IntegrationTest<PublicationModule, EditBookCommand>
    {
        private Guid _bookReaderGuid;
        private BookWriteModel _bookDto;
        private Book _aggregate;

        [Test]
        public async Task EditBook_WhenCalled_ShouldEditBook()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<BookByGuidQuery, BookDto>(new BookByGuidQuery(_aggregate.Guid));

            queryResult.Value.Guid.Should().Be(_bookDto.BookGuid);
            queryResult.Value.Authors.Should().HaveCount(_bookDto.Authors.Count);
            queryResult.Value.Cycles.Should().HaveCount(_bookDto.Cycles.Count);
            queryResult.Value.Isbn.Should().Be(_bookDto.Basics.Isbn);
            queryResult.Value.Title.Should().Be(_bookDto.Basics.Title);
            queryResult.Value.Publisher.Guid.Should().Be(_bookDto.Basics.PublisherGuid);
        }

        [Test]
        public async Task EditBook_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task EditBook_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command.WriteModel.BookWriteModel.Authors = new List<Guid>();
            Command.WriteModel.BookWriteModel.AddedBy = Guid.Empty;
            Command.WriteModel.BookWriteModel.BookGuid = Guid.Empty;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Authors");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "AddedBy");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
        }

        [Test]
        public async Task EditBook_WhenCalledAndBookIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var book = await UnitOfWork.GetAsync<Book>(_bookDto.BookGuid);

            var @event = BookArchived.Initialize()
                .WithAggregate(book.Guid)
                .WithPublisher(book.Publisher.PublisherGuid)
                .WithSeries(book.Series.SeriesGuid, book.Series.PositionInSeries)
                .WithAuthors(book.Authors.Select(s => s.AuthorGuid))
                .WithQuotes(book.BookQuotes.Select(s => s.QuoteGuid));

            book.ApplyChange(@event);

            await UnitOfWork.CommitAsync(book);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        protected override void InitializeRoot()
        {
            _bookReaderGuid = Fixture.Create<Guid>();

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

            PublicationModuleStartup.Initialize(
                new FakeHttpContextAccessor(_bookReaderGuid, true),
                appManagerMock.Object,
                new FakeLogger().GetLogger());
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

            var bookReader = new BookReader(Fixture.Create<Guid>(), _bookReaderGuid, bookReaderId);

            await UnitOfWork.CommitAsync(bookReader);

            var bookFactory = CompositionRoot.Kernel.Get<BookFactory>();

            var publisher = new Publisher(Fixture.Create<Guid>(), Fixture.Create<string>());

            await UnitOfWork.CommitAsync(publisher);

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();

            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            var bookData = this.CreateBookFactoryData(author.Guid, publisher.Guid);

            _aggregate = await bookFactory.CreateBook(bookData);

            await UnitOfWork.CommitAsync(_aggregate);

            _bookDto = Fixture.Build<BookWriteModel>()
                .With(p => p.BookGuid, _aggregate.Guid)
                .With(p => p.Description)
                .WithBookBasics(Category.NonFiction, SubCategory.NonFictionSubCategory.Design, "9788375155280",
                    Fixture.Create<string>(), publisher.Guid).WithDetails(Fixture.Create<int>(), Language.English)
                .WithCover(CoverType.HardCover, false)
                .WithSeries(Guid.Empty, default(byte))
                .With(p => p.AddedBy, bookReader.Guid)
                .With(p => p.Authors, new List<Guid> { author.Guid })
                .WithCycles(new List<Guid>())
                .Create<BookWriteModel>();

            Command = new EditBookCommand(new EditBookWriteModel
            {
                BookWriteModel = _bookDto,
                PictureWriteModel = new BookPictureWriteModel()
                {
                    Cover = string.Empty,
                    FileName = string.Empty
                }
            });
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
                .AddedBy(new BookReader(Fixture.Create<Guid>(), _bookReaderGuid, Fixture.Create<int>()))
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
                .AddedBy(this._bookReaderGuid)
                .WithCover(new BookCoverData(CoverType.PaperBack, Fixture.Create<string>()))
                .WithHashTags(Fixture.Create<List<string>>());
        }
    }
}