using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.IsbnValidation;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Domain.Books.Services.Editors;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Events.Book;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Services
{
    [TestFixture]
    public class BookEditorTests
    {
        private BookEditService _editService;

        private Book _book;

        private BookData _bookData;
        private Fixture _fixture;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [Test]
        public void EditBook_WhenCalledWithOnlyDifferentAuthors_ShouldChangeOnlyBookAuthors()
        {
            _book.AddAuthor(new BookAuthor(_fixture.Create<Guid>()));
            _book.CommitEvents();

            _editService.EditBook(_book, _bookData);

            _book.Should().NotBeNull();
            _book.Authors.Should().HaveCount(this._bookData.Authors.Count);

            var @events = _book.GetUncommittedEvents();
            @events.Where(p => p.GetType() == typeof(AuthorRemoved)).Should()
                .HaveCount(1); // because i have added one in arrange part;

            @events.Where(p => p.GetType() == typeof(AuthorAdded)).Should().HaveCount(0);
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentBasics_ShouldChangeOnlyBookBasics()
        {
            var basics = BookBasicsData.Initialize()
                .WithTitle(_bookData.BasicsData.Title)
                .WithIsbn(Isbn13Number())
                .WithDate(_fixture.Create<DateTime>())
                .WithCategory(new BookCategory(Category.NonFiction, SubCategory.NonFictionSubCategory.Design));

            this._bookData.WithBasics(
                basics,
                _bookData.PublisherGuid);

            _editService.EditBook(_book, this._bookData);

            _book.Should().NotBeNull();
            _book.Basics.Title.Should().Be(_bookData.BasicsData.Title);
            _book.Basics.BookCategory.Category.Value.Should().Be(_bookData.BasicsData.BookCategory.Category.Value);
            _book.Basics.BookCategory.SubCategory.Value.Should()
                .Be(_bookData.BasicsData.BookCategory.SubCategory.Value);
            _book.Basics.PublicationDate.Should().Be(_bookData.BasicsData.PublicationDate);
            _book.Basics.ISBN.Should().Be(_bookData.BasicsData.Isbn);

            var @events = _book.GetUncommittedEvents();
            @events.Count().Should().Be(1);
            @events.Should().ContainSingle(p => p is BookBasicsChanged);
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentDetails_ShouldChangeOnlyDetails()
        {
            this._bookData = this._bookData
                .WithDetails(new BookDetailsData(_fixture.Create<int>(), Language.English));

            _editService.EditBook(_book, _bookData);

            _book.Details.Should().NotBeNull();
            _book.Details.Language.Value.Should().Be(_bookData.DetailsData.Language.Value);
            _book.Details.Pages.Should().Be(_bookData.DetailsData.Pages);

            var @events = _book.GetUncommittedEvents();
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookDetailsChanged));
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentDescription_ShouldChangeOnlyDescription()
        {
            this._bookData =
                this._bookData.WithDescription(new BookDescriptionData(
                    _fixture.Create<string>(),
                    _fixture.Create<string>()));

            _editService.EditBook(_book, _bookData);

            _book.Should().NotBeNull();
            _book.Description.Should().NotBeNull();
            _book.Description.BookDescription.Should().Be(_book.Description.BookDescription);
            _book.Description.DescriptionSource.Should().Be(_book.Description.DescriptionSource);

            var @events = _book.GetUncommittedEvents();
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookDescriptionChanged));
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentCover_ShouldChangeOnlyBookCover()
        {
            this._bookData.WithCover(new BookCoverData(CoverType.HardCover, _fixture.Create<string>()));

            _editService.EditBook(_book, _bookData);

            _book.Should().NotBeNull();
            _book.Cover.Should().NotBeNull();
            _book.Cover.CoverType.Value.Should().Be(_bookData.CoverData.CoverType.Value);
            _book.Cover.CoverSource.Should().Be(_bookData.CoverData.CoverSource);

            var @events = _book.GetUncommittedEvents();
            @events.Count().Should().Be(1);
            @events.Should().ContainSingle(p => p is CoverChanged);
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentHasTags_ShouldChangeOnlyHashTags()
        {
            this._bookData.WithHashTags(_fixture.Create<List<string>>());

            _editService.EditBook(_book, _bookData);

            var @events = _book.GetUncommittedEvents();
            @events.Count().Should().Be(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookHashTagsChanged));
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentPublisher_ShouldChangeOnlyPublisher()
        {
            var publisherGuid = _fixture.Create<Guid>();

            _unitOfWorkMock.Setup(s => s.GetAsync<Publisher>(publisherGuid))
                .ReturnsAsync(new Publisher(publisherGuid, _fixture.Create<string>()));

            this._bookData.WithBasics(this._bookData.BasicsData, publisherGuid);

            _editService.EditBook(_book, _bookData);

            _book.Publisher.PublisherGuid.Should().Be(_bookData.PublisherGuid);

            var @events = _book.GetUncommittedEvents();
            @events.Count().Should().Be(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherChanged));
        }

        [Test]
        public void EditBook_WhenCalledWithDifferentSeries_ShouldChangeOnlySeries()
        {
            var seriesGuid = _fixture.Create<Guid>();

            _unitOfWorkMock.Setup(s => s.GetAsync<Series>(seriesGuid))
                .ReturnsAsync(new Series(seriesGuid, _fixture.Create<string>()));

            this._bookData.WithSeries(new BookSeriesData(seriesGuid, _fixture.Create<byte>()));

            _editService.EditBook(_book, _bookData);

            _book.Should().NotBeNull();
            _book.Series.Should().NotBeNull();
            _book.Series.SeriesGuid.Should().Be(_bookData.SeriesData.SeriesGuid);
            _book.Series.PositionInSeries.Should().Be(_bookData.SeriesData.PositionInSeries);

            var @events = _book.GetUncommittedEvents();
            @events.Count().Should().Be(1);
            @events.Should().ContainSingle(p => p is SeriesChanged);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            this._bookData = CreateBookData();

            var @event = _fixture.Create<BookCreated>()
                .WithAggregate(this._bookData.BookGuid)
                .WithAuthors(this._bookData.Authors)
                .WithTitleAndIsbn(_bookData.BasicsData.Title, Isbn13Number())
                .WithPublisher(this._bookData.PublisherGuid)
                .WithAddedBy(this._bookData.AddedByGuid)
                .WithCategory(
                    this._bookData.BasicsData.BookCategory.Category.Value,
                    this._bookData.BasicsData.BookCategory.Category.Name)
                .WithSubCategory(
                    this._bookData.BasicsData.BookCategory.SubCategory.Value,
                    this._bookData.BasicsData.BookCategory.SubCategory.Name)
                .WithCover(this._bookData.CoverData.CoverSource, this._bookData.CoverData.CoverType.Value,
                    this._bookData.CoverData.CoverType.Name)
                .WithHashTags(this._bookData.BookHashTags);

            _book = ReflectionHelper.CreateInstance(typeof(Book)) as Book;

            _book.ApplyChange(@event);

            _book.CommitEvents();

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(s => s.GetAsync<Publisher>(_bookData.PublisherGuid))
                .ReturnsAsync(new Publisher(_bookData.PublisherGuid, _fixture.Create<string>()));
            _unitOfWorkMock.Setup(s => s.GetAsync<Series>(_bookData.SeriesData.SeriesGuid))
                .ReturnsAsync(new Series(_bookData.SeriesData.SeriesGuid, _fixture.Create<string>()));
            _bookData.Authors.ForEach(guid =>
                _unitOfWorkMock.Setup(s => s.GetAsync<Author>(guid)).ReturnsAsync(new Author(
                    guid, new FullName(_fixture.Create<string>(), _fixture.Create<string>()), Sex.Male)));

            var validatorFactory = new IsbnValidatorFactory(new Dictionary<IsbnType, IIsbnValidator>()
            {
                { IsbnType.ISBN10, new IsbnTenValidator() },
                { IsbnType.ISBN13, new IsbnThirteenValidator() }
            });

            var editors = new List<IBookEditor>();
            editors.Add(new BookAuthorsEditor(_unitOfWorkMock.Object));
            editors.Add(new BookBasicsEditor(validatorFactory));
            editors.Add(new BookCoverEditor());
            editors.Add(new BookDescriptionEditor());
            editors.Add(new BookDetailsEditor());
            editors.Add(new BookHashTagsEditor());
            editors.Add(new BookPublisherEditor(_unitOfWorkMock.Object));
            editors.Add(new BookSeriesEditor(_unitOfWorkMock.Object));

            _editService = new BookEditService(editors);

            _editService.EditBook(_book, _bookData);

            _book.CommitEvents();
        }

        private string Isbn13Number()
        {
            return "9788381168724";
        }

        private BookData CreateBookData()
        {
            var category = new BookCategory(Category.Fiction, SubCategory.FictionSubCategory.Fantasy);

            var basics = BookBasicsData.Initialize()
                .WithTitle(_fixture.Create<string>())
                .WithIsbn(this.Isbn13Number())
                .WithCategory(category)
                .WithDate(_fixture.Create<DateTime>());

            var data = BookData.Initialize(_fixture.Create<Guid>(), _fixture.Create<List<Guid>>())
                .WithBasics(basics, _fixture.Create<Guid>())
                .WithDetails(new BookDetailsData(_fixture.Create<int>(), Language.Polish))
                .WithDescription(new BookDescriptionData(
                    _fixture.Create<string>(),
                    _fixture.Create<string>()))
                .WithSeries(
                    new BookSeriesData(_fixture.Create<Guid>(), _fixture.Create<byte>()))
                .WithCycles(_fixture.Create<List<Guid>>())
                .WithHashTags(_fixture.Create<List<string>>())
                .WithCover(new BookCoverData(CoverType.PaperBack, _fixture.Create<string>()))
                .AddedBy(_fixture.Create<Guid>());

            return data;
        }
    }
}