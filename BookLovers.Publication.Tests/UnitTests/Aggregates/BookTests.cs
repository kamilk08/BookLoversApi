using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.IsbnValidation;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Events.Book;
using BookLovers.Shared.Categories;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class BookTests
    {
        private Book _book;
        private IsbnValidatorFactory _factory;
        private Fixture _fixture;

        [Test]
        public void AddAuthor_WhenCalled_ShouldAddBookAuthor()
        {
            var authorGuid = _fixture.Create<Guid>();

            _book.AddAuthor(new BookAuthor(authorGuid));

            var @events = _book.GetUncommittedEvents();

            _book.Should().NotBeNull();
            _book.Authors.Should().HaveCount(2);
            _book.Authors.Should().Contain(p => p.AuthorGuid == authorGuid);

            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorAdded));
        }

        [Test]
        public void AddAuthor_WhenCalledWithInActiveBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            var authorGuid = _fixture.Create<Guid>();

            Action act = () => _book.AddAuthor(new BookAuthor(authorGuid));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddAuthor_WhenCalledAuthorThatBookAlreadyHave_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorGuid = _fixture.Create<Guid>();
            _book.AddAuthor(new BookAuthor(authorGuid));
            _book.CommitEvents();

            Action act = () => _book.AddAuthor(new BookAuthor(authorGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book already contains selected author");
        }

        [Test]
        public void RemoveAuthor_WhenCalled_ShouldRemoveAuthor()
        {
            var authorGuid = _fixture.Create<Guid>();

            _book.AddAuthor(new BookAuthor(authorGuid));
            _book.CommitEvents();

            var bookAuthor = _book.GetBookAuthor(authorGuid);

            _book.RemoveAuthor(bookAuthor);

            var @events = _book.GetUncommittedEvents();

            _book.Authors.Should().HaveCount(1);
            _book.Authors.Should().NotContain(p => p.AuthorGuid == authorGuid);

            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorRemoved));
        }

        [Test]
        public void RemoveAuthor_WhenCalledWithArchivedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            Action act = () => _book.RemoveAuthor(new BookAuthor(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveAuthor_WhenCalledWithAuthorThatDoesNotExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorGuid = _fixture.Create<Guid>();

            Action act = () => _book.RemoveAuthor(new BookAuthor(authorGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Cannot remove author from book.Book does not contain selected author");
        }

        [Test]
        public void AddReview_WhenCalled_ShouldAddReviewToBook()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            _book.AddReview(new BookReview(readerGuid, reviewGuid));

            _book.Should().NotBeNull();
            _book.Reviews.Should().HaveCount(1);
            _book.Reviews.Should().Contain(p => p.ReaderGuid == readerGuid && p.ReviewGuid == reviewGuid);

            var @events = _book.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookReviewAdded));
        }

        [Test]
        public void AddReview_WhenCalledButBookAlreadyHaveCertainReview_ShouldThrowBusinessRuleNotMeetException()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            var bookReview = new BookReview(readerGuid, reviewGuid);

            _book.AddReview(bookReview);
            _book.CommitEvents();

            Action act = () => _book.AddReview(bookReview);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader already added review to selected book. Cannot duplicate reviews");
        }

        [Test]
        public void AddReview_WhenCalledWithInActiveBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            var bookReview = new BookReview(readerGuid, reviewGuid);

            Action act = () => _book.AddReview(bookReview);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveReview_WhenCalled_ShouldRemoveBookReview()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            _book.AddReview(new BookReview(readerGuid, reviewGuid));
            _book.CommitEvents();

            var bookReview = _book.GetBookReview(readerGuid);

            _book.RemoveReview(bookReview);

            var @events = _book.GetUncommittedEvents();

            _book.Reviews.Should().HaveCount(0);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookReviewRemoved));
        }

        [Test]
        public void RemoveReview_WhenCalledWithArchivedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            var bookReview = new BookReview(readerGuid, reviewGuid);

            Action act = () => _book.RemoveReview(bookReview);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveReview_WhenCalledButBookDoesNotHaveCertainReview_ShouldThrowBusinessRuleNotMeetExcetpion()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            var bookReview = new BookReview(readerGuid, reviewGuid);

            Action act = () => _book.RemoveReview(bookReview);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book has no review from selected reader");
        }

        [Test]
        public void ChangePublisher_WhenCalled_ShouldChangeBookPublisher()
        {
            var publisherGuid = _fixture.Create<Guid>();

            _book.ChangePublisher(new BookPublisher(publisherGuid));

            _book.Should().NotBeNull();
            _book.Publisher.Should().NotBeNull();
            _book.Publisher.PublisherGuid.Should().Be(publisherGuid);

            var @events = _book.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherChanged));
        }

        [Test]
        public void ChangePublisher_WhenCalledWithArchivedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            Action act = () => _book.ChangePublisher(new BookPublisher(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ChangeSeries_WhenCalled_ShouldChangeBookSeries()
        {
            var seriesGuid = _fixture.Create<Guid>();
            var bookSeries = new BookSeries(seriesGuid, _fixture.Create<byte>());
            _book.ChangeSeries(bookSeries);

            _book.Should().NotBeNull();
            _book.Series.Should().NotBeNull();
            _book.Series.SeriesGuid.Should().Be(seriesGuid);
            _book.Series.PositionInSeries.Should().Be(bookSeries.PositionInSeries);

            var @events = _book.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(SeriesChanged));
        }

        [Test]
        public void ChangeSeries_WhenCalledWithArchivedBook_ShouldThrowBusinessRuleNotMeetExecption()
        {
            var @event = _fixture.Create<BookArchived>()
                .WithAggregate(_book.Guid)
                .WithAuthors(_book.Authors.Select(s => s.AuthorGuid).ToList())
                .WithPublisher(_book.Publisher.PublisherGuid)
                .WithSeries(_book.Series.SeriesGuid, _book.Series.PositionInSeries);

            _book.ApplyChange(@event);
            _book.CommitEvents();

            Action act = () => _book.ChangeSeries(new BookSeries(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void GetBookReview_WhenCalled_ShouldReturnBookReview()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewGuid = _fixture.Create<Guid>();

            var bookReview = new BookReview(readerGuid, reviewGuid);
            _book.AddReview(bookReview);

            var review = _book.GetBookReview(readerGuid);
            review.Should().NotBeNull();
            review.Should().BeEquivalentTo(bookReview);
        }

        [Test]
        public void GetBookReview_WhenCalledWithMissingReview_ShouldReturnNull()
        {
            var bookReview = _book.GetBookReview(_fixture.Create<Guid>());

            bookReview.Should().BeNull();
        }

        [Test]
        public void GetBookAuthor_WhenCalled_ShouldReturnBookAuthor()
        {
            var bookAuthor = new BookAuthor(_fixture.Create<Guid>());
            _book.AddAuthor(bookAuthor);
            _book.CommitEvents();

            var author = _book.GetBookAuthor(bookAuthor.AuthorGuid);
            author.Should().NotBeNull();
            author.Should().BeEquivalentTo(bookAuthor);
        }

        [Test]
        public void GetBookAuthor_WhenCalledWithMissingAuthor_ShouldReturnNull()
        {
            var author = _book.GetBookAuthor(_fixture.Create<Guid>());

            author.Should().BeNull();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            this._factory = new IsbnValidatorFactory(new Dictionary<IsbnType, IIsbnValidator>
            {
                { IsbnType.ISBN10, new IsbnTenValidator() },
                { IsbnType.ISBN13, new IsbnThirteenValidator() }
            });

            _book = (Book) Activator.CreateInstance(typeof(Book), true);
            var bookCreated = BookCreated.Initialize()
                .WithAggregate(_fixture.Create<Guid>())
                .WithAuthors(new List<Guid>() { Guid.NewGuid() })
                .WithTitleAndIsbn("TITLE", "978-83-7515-495-5")
                .WithPublisher(_fixture.Create<Guid>())
                .WithAddedBy(_fixture.Create<Guid>())
                .WithDetails(100, Language.Polish.Value, "HUJ")
                .WithCategory(Category.Fiction.Value, Category.Fiction.Name)
                .WithCover("COVER_SOURCE", 1, "COVER TYPE")
                .WithCycles(new List<Guid> { Guid.NewGuid() })
                .WithDescription("BOOK_DESCRIPTION", "BOOK_DESCRIPTION_SOURCE")
                .WithSubCategory(
                    SubCategory.FictionSubCategory.Fantasy.Value,
                    SubCategory.FictionSubCategory.Fantasy.Name)
                .WithPublicationDate(new DateTime(1992, 12, 12))
                .WithSeries(Guid.NewGuid(), 1)
                .WithHashTags(new List<string>());

            _book.ApplyChange(bookCreated);

            _book.CommitEvents();
        }
    }
}