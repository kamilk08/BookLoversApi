using System;
using System.Collections.Generic;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.Publisher;
using BookLovers.Ratings.Domain.PublisherCycles;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.UnitTests
{
    [TestFixture]
    public class PublisherTests
    {
        private Fixture _fixture;
        private Publisher _publisher;

        [Test]
        public void AddBook_WhenCalled_ShouldAddNewBook()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisher.AddBook(book);

            this._publisher.Books.Should().HaveCount(1);
            this._publisher.Books.Should().Contain(book);
        }

        [Test]
        public void AddBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisher.ArchiveAggregate();

            var authors = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            Action act = () => this._publisher.AddBook(Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()), authors));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledAndPublisherAlreadyHasSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisher.AddBook(book);

            Action act = () => this._publisher.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cannot have multiple same books.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemovePublisherBook()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisher.AddBook(book);

            this._publisher.Books.Should().HaveCount(1);
            this._publisher.Books.Should().Contain(book);

            this._publisher.RemoveBook(book);

            this._publisher.Books.Should().HaveCount(0);
            this._publisher.Books.Should().NotContain(book);
        }

        [Test]
        public void RemoveBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisher.ArchiveAggregate();

            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._publisher.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalled_AndPublisherDoesNotHaveSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._publisher.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher does not contain selected book.");
        }

        [Test]
        public void AddCycle_WhenCalled_ShouldAddNewCycleToPublisher()
        {
            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            this._publisher.AddCycle(publisherCycle);

            this._publisher.PublisherCycles.Should().HaveCount(1);
            this._publisher.PublisherCycles.Should().Contain(publisherCycle);
        }

        [Test]
        public void AddCycle_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisher.ArchiveAggregate();

            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            Action act = () => this._publisher.AddCycle(publisherCycle);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddCycle_WhenCalledAndPublisherHasSelectedCycle_ShouldThrowBusinessRuleNotMeetException()
        {
            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            this._publisher.AddCycle(publisherCycle);

            Action act = () => this._publisher.AddCycle(publisherCycle);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cannot have duplicated cycles.");
        }

        [Test]
        public void RemoveCycle_WhenCalled_ShouldRemovePublisherBook()
        {
            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            this._publisher.AddCycle(publisherCycle);

            this._publisher.PublisherCycles.Should().HaveCount(1);
            this._publisher.PublisherCycles.Should().Contain(publisherCycle);

            this._publisher.RemoveCycle(publisherCycle);

            this._publisher.PublisherCycles.Should().HaveCount(0);
            this._publisher.PublisherCycles.Should().NotContain(publisherCycle);
        }

        [Test]
        public void RemoveCycle_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisher.ArchiveAggregate();

            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            Action act = () => this._publisher.RemoveCycle(publisherCycle);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveCycle_WhenCalledAndPublisherDoesNotHaveSelectedCycle_ShouldThrowBusinessRuleNotMeetException()
        {
            var publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));

            Action act = () => this._publisher.RemoveCycle(publisherCycle);

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Publisher does not have selected cycle");
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            this._publisher = Publisher.Create(
                new PublisherIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));
        }
    }
}