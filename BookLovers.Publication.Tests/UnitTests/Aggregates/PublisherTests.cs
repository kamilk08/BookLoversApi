using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Events.Publishers;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class PublisherTests
    {
        private Publisher _publisher;
        private Fixture _fixture;

        [Test]
        public void AddBook_WhenCalled_ShouldAddBookToPublishersCollection()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisher.AddBook(new PublisherBook(bookGuid));

            _publisher.Books.Should().HaveCount(1);
            _publisher.Books.Should().ContainSingle(p => p.BookGuid == bookGuid);

            var @events = _publisher.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherBookAdded));
        }

        [Test]
        public void AddBook_WhenCalledWithArchivedPublisher_ShouldThrowBussinesRuleNotMeetException()
        {
            var @event = new PublisherArchived(_publisher.Guid, _publisher.Books.Select(s => s.BookGuid).ToList());

            _publisher.ApplyChange(@event);

            Action act = () => _publisher.AddBook(new PublisherBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledAndPublisherAlreadyHaveSelectedBooks_ShouldThrowBussinesRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisher.AddBook(new PublisherBook(bookGuid));

            Action act = () => _publisher.AddBook(new PublisherBook(bookGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cannot have duplicated books.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemovePublisherBook()
        {
            var bookGuid = _fixture.Create<Guid>();

            _publisher.AddBook(new PublisherBook(bookGuid));
            _publisher.CommitEvents();

            var book = _publisher.GetBook(bookGuid);

            _publisher.RemoveBook(book);

            _publisher.Books.Should().HaveCount(0);

            var @events = _publisher.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherBookRemoved));
        }

        [Test]
        public void RemoveBook_WhenCalledWithPublisherArchived_BussinesRuleNotMeetException()
        {
            var @event = new PublisherArchived(_publisher.Guid, _publisher.Books.Select(s => s.BookGuid).ToList());

            _publisher.ApplyChange(@event);

            Action act = () => _publisher.RemoveBook(new PublisherBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndPublisherDoesNotHaveSelectedBook_ShouldBussinesRuleNotMeetException()
        {
            Action act = () => _publisher.RemoveBook(new PublisherBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher does not have selected book.");
        }

        [Test]
        public void RemoveCycle_WhenCalled_ShouldRemoveCycle()
        {
            var cycleGuid = _fixture.Create<Guid>();

            _publisher.AddCycle(new Cycle(cycleGuid));
            _publisher.CommitEvents();

            var cycle = _publisher.GetCycle(cycleGuid);

            _publisher.RemoveCycle(cycle);

            _publisher.Cycles.Should().HaveCount(0);

            var @events = _publisher.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherCycleRemoved));
        }

        [Test]
        public void RemoveCycle_WhenCalledWithArchivedPublisher_ShouldThrowBussinesRuleNotMeetException()
        {
            var @event = new PublisherArchived(_publisher.Guid, _publisher.Books.Select(s => s.BookGuid).ToList());

            _publisher.ApplyChange(@event);

            Action act = () => _publisher.RemoveCycle(new Cycle(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveCycle_WhenCalledAndPublisherDoesNotHaveSelectedCycle_ShouldThrowBussinesRuleNotMeetException()
        {
            Action act = () => _publisher.RemoveCycle(new Cycle(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage($"Publisher does not have selected cycle.");
        }

        [Test]
        public void AddCycle_WhenCalled_ShouldAddCycle()
        {
            var cycleGuid = _fixture.Create<Guid>();

            _publisher.AddCycle(new Cycle(cycleGuid));

            _publisher.Cycles.Should().HaveCount(1);
            _publisher.Cycles.Should().ContainSingle(p => p.CycleGuid == cycleGuid);

            var @events = _publisher.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(PublisherCycleAdded));
        }

        [Test]
        public void AddCycle_WhenCalledWithArchivedPublisher_ShouldThrowBussinesRuleNotMeetException()
        {
            _publisher.ApplyChange(new PublisherArchived(_publisher.Guid, _publisher.Books.Select(s => s.BookGuid)
                .ToList()));

            Action act = () => _publisher.AddCycle(new Cycle(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddCycle_WhenCalledButPublisherAlreadyHaveCycle_ShouldThrowBussinesRuleNotMeetException()
        {
            var cycleGuid = _fixture.Create<Guid>();

            _publisher.AddCycle(new Cycle(cycleGuid));

            Action act = () => _publisher.AddCycle(new Cycle(cycleGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cannot have duplicated cycles.");
        }

        [Test]
        public void GetCycle_WhenCalled_ShouldReturnCycle()
        {
            var cycleGuid = _fixture.Create<Guid>();
            _publisher.AddCycle(new Cycle(cycleGuid));

            var cycle = _publisher.GetCycle(cycleGuid);

            cycle.Should().NotBeNull();
            cycle.CycleGuid.Should().Be(cycleGuid);
        }

        [Test]
        public void GetCycle_WhenCalled_ShouldReturnNull()
        {
            var cycle = _publisher.GetCycle(_fixture.Create<Guid>());

            cycle.Should().BeNull();
        }

        [Test]
        public void GetBook_WhenCalled_ShouldReturnBook()
        {
            var bookGuid = _fixture.Create<Guid>();
            var publisherBook = new PublisherBook(bookGuid);

            _publisher.AddBook(publisherBook);

            var book = _publisher.GetBook(bookGuid);

            book.Should().NotBeNull();
            book.BookGuid.Should().Be(bookGuid);
        }

        [Test]
        public void GetBook_WhenCalled_ShouldReturnNull()
        {
            var book = _publisher.GetBook(_fixture.Create<Guid>());

            book.Should().BeNull();
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            _publisher = new Publisher(_fixture.Create<Guid>(), _fixture.Create<string>());

            _publisher.CommitEvents();
        }
    }
}