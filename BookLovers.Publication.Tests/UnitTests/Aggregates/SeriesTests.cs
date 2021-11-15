using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Events.SeriesCycle;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class SeriesTests
    {
        private Series _series;
        private Fixture _fixture;

        [Test]
        [TestCase(1)]
        [TestCase(255)]
        [TestCase(2)]
        public void AddToSeries_WhenCalled_ShouldAddBookToSeries(byte positionInSeries)
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, positionInSeries));
            var @events = _series.GetUncommittedEvents();

            _series.Books.Should().HaveCount(1);
            _series.Books.Should().ContainSingle(p => p.Position == positionInSeries && p.BookGuid == bookGuid);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(AddedToSeries));
        }

        [Test]
        public void AddToSeries_WhenCalledAndSeriesIsArchived_ShouldThrowBussinesRuleNotMeetException()
        {
            var @event = new SeriesArchived(_series.Guid, _series.Books.Select(s => s.BookGuid).ToList());

            _series.ApplyChange(@event);

            _series.CommitEvents();

            Action act = () => _series.AddToSeries(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddToSeries_WhenCalledAndSeriesAlreadyHaveSelectedBook_ShouldThrowBussinesRuleNotMeetEception()
        {
            var bookGuid = _fixture.Create<Guid>();
            var position = _fixture.Create<byte>();

            _series.AddToSeries(new SeriesBook(bookGuid, position));

            Action act = () => _series.AddToSeries(new SeriesBook(bookGuid, position));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book series cannot contain duplciated book.");
        }

        [Test]
        [TestCase(0)]
        public void AddToSeries_WhenCalledWithInvalidSeriesPosition_ShouldThrowSeriesInvalidPositionException(
            byte position)
        {
            Action act = () => _series.AddToSeries(new SeriesBook(_fixture.Create<Guid>(), position));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book position cannot be less than one.");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(255)]
        public void RemoveFromSeries_WhenCalled_ShouldRemoveBookFromSeries(byte positionInSeries)
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, positionInSeries));
            _series.CommitEvents();

            _series.RemoveBook(new SeriesBook(bookGuid, positionInSeries));

            _series.Books.Should().HaveCount(0);

            var @events = _series.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookRemovedFromSeries));
        }

        [Test]
        public void RemoveFromSeries_WhenCalledAndSeriesIsArchived_ShouldThrowBussinesRuleNotMeetException()
        {
            var @event = new SeriesArchived(_series.Guid, _series.Books.Select(s => s.BookGuid).ToList());
            _series.ApplyChange(@event);

            Action act = () =>
                _series.RemoveBook(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveFromSeries_WhenCalledAndSeriesBookIsMissing_ShouldThrowBussinesRuleNotMeetException()
        {
            Action act = () =>
                _series.RemoveBook(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Series must contain selected book.");
        }

        [Test]
        [TestCase(2)]
        [TestCase(255)]
        public void ChangePosition_WhenCalled_ShouldChangeBooksPositionInSeries(byte positionInSeries)
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, 1));
            _series.CommitEvents();

            _series.ChangePosition(new SeriesBook(bookGuid, positionInSeries));

            var @events = _series.GetUncommittedEvents();

            _series.Books.Should().HaveCount(1);
            _series.Books.Should().ContainSingle(p => p.BookGuid == bookGuid && p.Position == positionInSeries);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookPositionInSeriesChanged));
        }

        [Test]
        public void ChangePosition_WhenCalledAndSeriesBookIsMissing_ShouldThrowBussinesRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();

            Action act = () => _series.ChangePosition(new SeriesBook(bookGuid, _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Series must contain selected book.");
        }

        [Test]
        public void ChangePosition_WhenCalledAndSeriesIsArchived_ShouldThrowBussinesRuleNotMeetException()
        {
            _series.ApplyChange(new SeriesArchived(_series.Guid, _series.Books.Select(s => s.BookGuid).ToList()));

            Action act = () => _series.ChangePosition(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(255)]
        public void ChangeBookPosition_SeriesPositionTaken_ShouldThrowSeriesPositionTakenException(
            byte positionInSeries)
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, positionInSeries));

            Action act = () => _series.ChangePosition(new SeriesBook(bookGuid, positionInSeries));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Cannot add book to position that is already taken by other book.");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(255)]
        public void GetPositionInSeries_WhenCalled_ShouldReturnBookPositionInSeries(byte positionInSeries)
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, positionInSeries));

            var position = _series.GetPositionInSeries(bookGuid);

            position.Should().Be(positionInSeries);
            _series.Books.Should().ContainSingle(p => p.Position == position);
        }

        [Test]
        public void GetPositionInSeries_WhenCalled_ShouldReturnPositionThatDoesNotExists()
        {
            var position = _series.GetPositionInSeries(_fixture.Create<Guid>());

            position.Should().Be(0);
        }

        [Test]
        public void GetBook_WhenCalledWithBookGuid_ShouldReturnBook()
        {
            var bookGuid = _fixture.Create<Guid>();

            _series.AddToSeries(new SeriesBook(bookGuid, _fixture.Create<byte>()));

            var book = _series.GetBook(bookGuid);

            book.BookGuid.Should().Be(bookGuid);
        }

        [Test]
        public void GetBook_WhenCalledWithPosition_ShouldReturnBookAtGivenPostion()
        {
            var position = _fixture.Create<byte>();

            var seriesBook = new SeriesBook(_fixture.Create<Guid>(), position);

            _series.AddToSeries(seriesBook);

            var book = _series.GetBook(position);

            book.Should().NotBeNull();
            book.Should().BeEquivalentTo(seriesBook);
        }

        [SetUp]
        public void Setup()
        {
            this._fixture = new Fixture();

            _series = new Series(_fixture.Create<Guid>(), _fixture.Create<string>());

            _series.CommitEvents();
        }
    }
}