using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Events.PublisherCycles;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class CycleTests
    {
        private PublisherCycle _publisherCycle;
        private Fixture _fixture;

        [Test]
        public void AddBook_WhenCalled_ShouldAddBookToCycle()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisherCycle.AddBook(new CycleBook(bookGuid));

            var @events = _publisherCycle.GetUncommittedEvents();

            _publisherCycle.Books.Should().HaveCount(1);
            _publisherCycle.Books.Should().ContainSingle(p => p.BookGuid == bookGuid);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookAddedToCycle));
        }

        [Test]
        public void AddBook_WhenCalledWithCycleArchived_ShouldThrowBusinessRuleNotMeetException()
        {
            _publisherCycle.ApplyChange(new PublisherCycleArchived(
                _publisherCycle.Guid,
                _publisherCycle.PublisherGuid));

            _publisherCycle.CommitEvents();

            Action act = () => _publisherCycle.AddBook(new CycleBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledButCycleAlreadyHaveSelectedBook_ShouldThrowBussinesRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisherCycle.AddBook(new CycleBook(bookGuid));

            Action act = () => _publisherCycle.AddBook(new CycleBook(bookGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Cannot add duplicated book to publisher cycle.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemoveBookFromCycle()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisherCycle.AddBook(new CycleBook(bookGuid));
            _publisherCycle.CommitEvents();

            var book = _publisherCycle.GetCycleBook(bookGuid);

            _publisherCycle.RemoveBook(book);

            var @events = _publisherCycle.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(BookRemovedFromCycle));
        }

        [Test]
        public void RemoveBook_WhenCalledWithCycleArchived_ShouldThrowBusinessRuleNotMeetException()
        {
            _publisherCycle.ApplyChange(new PublisherCycleArchived(
                _publisherCycle.Guid,
                _publisherCycle.PublisherGuid));

            _publisherCycle.CommitEvents();

            Action act = () => _publisherCycle.RemoveBook(new CycleBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndCycleDoesNotHaveSelectedBook_ShouldThrowBussinesRuleNotMeetException()
        {
            Action act = () => _publisherCycle.RemoveBook(new CycleBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cycle must contain selected book.");
        }

        [Test]
        public void GetCycleBook_WhenCalled_ShouldReturnCycleBook()
        {
            var cycleBook = new CycleBook(_fixture.Create<Guid>());

            _publisherCycle.AddBook(cycleBook);

            var result = _publisherCycle.GetCycleBook(cycleBook.BookGuid);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(cycleBook);
        }

        [Test]
        public void GetCycleBook_WhenCalled_ShouldReturnNull()
        {
            var result = _publisherCycle.GetCycleBook(_fixture.Create<Guid>());

            result.Should().BeNull();
        }

        [Test]
        public void HasBook_WhenCalled_ShouldReturnTrue()
        {
            var cycleBook = new CycleBook(_fixture.Create<Guid>());

            _publisherCycle.AddBook(cycleBook);

            var has = _publisherCycle.HasBook(cycleBook.BookGuid);

            has.Should().BeTrue();
        }

        [Test]
        public void HasBook_WhenCalled_ShouldReturnFalse()
        {
            var has = _publisherCycle.HasBook(_fixture.Create<Guid>());

            has.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _publisherCycle = new PublisherCycle(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                _fixture.Create<string>());

            _publisherCycle.CommitEvents();
        }
    }
}