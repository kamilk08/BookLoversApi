using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.IsbnValidation;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Events.Book;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Services
{
    [TestFixture]
    public class BookFactoryTests
    {
        private BookFactory _bookFactory;
        private BookData _bookData;
        private Fixture _fixture;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ITitleUniquenessChecker> _titleUniquenessCheckerMock;
        private Mock<IIsbnUniquenessChecker> _isbnUniquenessCheckerMock;
        private Guid _addedBy;

        [Test]
        public void CreateBook_WhenCalled_ShouldReturnCreatedBook()
        {
            Book book = null;

            Func<Task> act = async () => book = await _bookFactory.CreateBook(this._bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Should().NotBeNull();
            book.Guid.Should().Be(_bookData.BookGuid);
            book.Reviews.Should().NotBeNull();
            book.Reviews.Should().HaveCount(0);
            book.Series.SeriesGuid.Should().Be(_bookData.SeriesData.SeriesGuid);
            book.Series.PositionInSeries.Should().Be(_bookData.SeriesData.PositionInSeries);
            book.Publisher.PublisherGuid.Should().Be(_bookData.PublisherGuid);
            book.Authors.Should().NotBeNull();
            book.Authors.Should().HaveCount(_bookData.Authors.Count);
            book.Authors.Should().OnlyHaveUniqueItems();
            book.Basics.Title.Should().BeEquivalentTo(_bookData.BasicsData.Title);
            book.Basics.PublicationDate.Should().Be(_bookData.BasicsData.PublicationDate);
            book.Basics.ISBN.Should().Be(_bookData.BasicsData.Isbn);
            book.Basics.BookCategory.Category.Value.Should().Be(_bookData.BasicsData.BookCategory.Category.Value);
            book.Basics.BookCategory.SubCategory.Value.Should().Be(_bookData.BasicsData.BookCategory.SubCategory.Value);
            book.Description.BookDescription.Should().Be(_bookData.DescriptionData.Content);
            book.Description.DescriptionSource.Should().BeEquivalentTo(_bookData.DescriptionData.DescriptionSource);
            book.Details.Language.Value.Should().Be(_bookData.DetailsData.Language.Value);
            book.Details.Pages.Should().Be(_bookData.DetailsData.Pages);

            var @events = book.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<BookCreated>();
        }

        [Test]
        [TestCase(null, null, null)]
        [TestCase("978-83-7515-495-5", null, null)]
        [TestCase("978-83-7515-495-5", null, 100)]
        [TestCase("978-83-7515-495-5", 12, 100)]
        public void CreateBook_WhenCalledWithoutValidBasics_ShouldThrowBusinessRuleNotMeetException(
            string isbn,
            int? category,
            int? subCategory)
        {
            var bookBasicsData = BookBasicsData.Initialize()
                .WithTitle(this._bookData.BasicsData.Title)
                .WithIsbn(isbn)
                .WithCategory(new BookCategory(category.GetValueOrDefault(), subCategory.GetValueOrDefault()))
                .WithDate(_fixture.Create<DateTime>());

            this._bookData = this._bookData.WithBasics(bookBasicsData, this._bookData.PublisherGuid);

            Func<Task> act = async () => await _bookFactory.CreateBook(this._bookData);
            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void CreateBook_WhenCalledWithoutDetails_ShouldReturnBookWithDefaultNumberOfPagesAndDefaultLanguage()
        {
            this._bookData = this._bookData.WithDetails(new BookDetailsData(default(int), Language.Unknown));

            Book book = null;

            Func<Task> act = async () => book = await _bookFactory.CreateBook(_bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Should().NotBeNull();
            book.Details.Should().NotBeNull();
            book.Details.Language.Should().BeEquivalentTo(Language.Unknown);
            book.Details.Pages.Should().Be(BookDetails.MinimalAmountOfPages);

            book.GetUncommittedEvents().Should().HaveCount(1);
            book.GetUncommittedEvents().Should().AllBeOfType<BookCreated>();
        }

        [Test]
        public void CreateBook_WhenCalledWithoutCycles_ShouldReturnBookThatIsNotPartOfAnyCycle()
        {
            _unitOfWorkMock.Setup(s => s.GetMultipleAsync<PublisherCycle>(new List<Guid>()))
                .ReturnsAsync(new List<PublisherCycle>());

            this._bookData = this._bookData.WithCycles(new List<Guid>());

            Book book = null;

            Func<Task> act = async () => book = await _bookFactory.CreateBook(_bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Should().NotBeNull();

            var @events = book.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<BookCreated>();

            ((BookCreated) @events.First()).Cycles.Should().HaveCount(0);
        }

        [Test]
        public void CreateBook_WhenCalledWithoutDescription_ShouldReturnBookWithoutDescription()
        {
            this._bookData = _bookData.WithDescription(new BookDescriptionData(null, null));

            Book book = null;

            Func<Task> act = async () => book = await _bookFactory.CreateBook(this._bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Description.Should().NotBeNull();
            book.Description.BookDescription.Should().BeNull();
            book.Description.DescriptionSource.Should().BeNull();
        }

        [Test]
        public void CreateBook_WhenCalledWithoutCover_ShouldReturnBookWithNoCover()
        {
            this._bookData = this._bookData.WithCover(new BookCoverData(CoverType.NoCover, string.Empty));

            Book book = null;
            Func<Task> act = async () => book = await _bookFactory.CreateBook(_bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Should().NotBeNull();
            book.Cover.Should().NotBeNull();
            book.Cover.CoverSource.Should().Be(_bookData.CoverData.CoverSource);
            book.Cover.CoverType.Value.Should().Be(_bookData.CoverData.CoverType.Value);
        }

        [Test]
        public void CreateBook_WhenCalledWithoutSeries_ShouldReturnBookWithNoSeries()
        {
            var bookData = this._bookData.WithSeries(new BookSeriesData(null, null));
            Book book = null;

            Func<Task> act = async () => book = await _bookFactory.CreateBook(bookData);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            book.Should().NotBeNull();
            book.Series.Should().NotBeNull();
            book.Series.SeriesGuid.Should().BeEmpty();
            book.Series.PositionInSeries.Should().Be(default(int));
        }

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            var publisherGuid = _fixture.Create<Guid>();
            var authorsGuides = _fixture.Create<List<Guid>>();
            var seriesGuid = _fixture.Create<Guid>();
            var cycleGuides = _fixture.Create<List<Guid>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _titleUniquenessCheckerMock = new Mock<ITitleUniquenessChecker>();
            _isbnUniquenessCheckerMock = new Mock<IIsbnUniquenessChecker>();
            _addedBy = _fixture.Create<Guid>();

            var mock = new Mock<IBookReaderAccessor>();

            mock.Setup(s => s.GetAggregateGuidAsync(It.IsAny<Guid>())).ReturnsAsync(_addedBy);

            var author = (Author) ReflectionHelper.CreateInstance(typeof(Author));

            author.ApplyChange(AuthorCreated.Initialize()
                .WithAggregate(_fixture.Create<Guid>())
                .WithDescription(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>())
                .WithDetails(_fixture.Create<string>(), _fixture.Create<DateTime>(), _fixture.Create<DateTime>())
                .WithSex(Sex.Male.Value).WithAddedBy(_addedBy)
                .WithFullName(_fixture.Create<string>(), _fixture.Create<string>()));

            _unitOfWorkMock.Setup(s => s.GetAsync<Publisher>(publisherGuid))
                .ReturnsAsync(new Publisher(publisherGuid, _fixture.Create<string>()));

            _unitOfWorkMock.Setup(s => s.GetMultipleAsync<Author>(authorsGuides)).ReturnsAsync(new List<Author>()
            {
                author
            });

            _unitOfWorkMock.Setup(s => s.GetAsync<Series>(seriesGuid))
                .ReturnsAsync(new Series(seriesGuid, _fixture.Create<string>()));

            _unitOfWorkMock.Setup(s => s.GetMultipleAsync<PublisherCycle>(cycleGuides)).ReturnsAsync(cycleGuides
                .Select(s => new PublisherCycle(s, publisherGuid, _fixture.Create<string>())).ToList());

            _bookData = CreateBookData(authorsGuides, publisherGuid, seriesGuid, cycleGuides);

            _titleUniquenessCheckerMock.Setup(s => s.IsUnique(_bookData.BasicsData.Title)).Returns(() => true);

            _isbnUniquenessCheckerMock.Setup(s => s.IsUnique(_bookData.BasicsData.Isbn)).Returns(() => true);

            _bookFactory = new BookFactory(_unitOfWorkMock.Object, new BookBuilder(), new IsbnValidatorFactory(
                new Dictionary<IsbnType, IIsbnValidator>()
                {
                    {
                        IsbnType.ISBN10,
                        new IsbnTenValidator()
                    },
                    {
                        IsbnType.ISBN13,
                        new IsbnThirteenValidator()
                    }
                }), _titleUniquenessCheckerMock.Object, _isbnUniquenessCheckerMock.Object, mock.Object);
        }

        private BookData CreateBookData(
            List<Guid> authorGuids,
            Guid publisherGuid,
            Guid seriesGuid,
            List<Guid> cycles)
        {
            var category = new BookCategory(Category.Fiction, SubCategory.FictionSubCategory.Fantasy);

            var basicsData = BookBasicsData.Initialize().WithTitle(_fixture.Create<string>()).WithIsbn(Isbn13Number())
                .WithCategory(category).WithDate(_fixture.Create<DateTime>());

            return BookData.Initialize(_fixture.Create<Guid>(), authorGuids).WithBasics(basicsData, publisherGuid)
                .WithDetails(new BookDetailsData(_fixture.Create<int>(), Language.Polish))
                .WithDescription(new BookDescriptionData(_fixture.Create<string>(), _fixture.Create<string>()))
                .WithSeries(new BookSeriesData(seriesGuid, _fixture.Create<byte>())).WithCycles(cycles)
                .WithHashTags(_fixture.Create<List<string>>())
                .WithCover(new BookCoverData(CoverType.PaperBack, _fixture.Create<string>())).AddedBy(_addedBy);
        }

        private string Isbn13Number()
        {
            return "9788381168724";
        }

        private string Isbn10Number()
        {
            return "2123456802";
        }
    }
}