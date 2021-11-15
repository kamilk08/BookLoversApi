using System;
using System.Collections.Generic;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.PublisherCycles;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.UnitTests
{
    [TestFixture]
    public class PublisherCycleTests
    {
        private Fixture _fixture;
        private PublisherCycle _publisherCycle;

        [Test]
        public void AddBook_WhenCalled_ShouldAddPublisherCycle()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisherCycle.AddBook(book);

            this._publisherCycle.Books.Should().HaveCount(1);
            this._publisherCycle.Books.Should().Contain(book);
        }

        [Test]
        public void AddBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisherCycle.ArchiveAggregate();

            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._publisherCycle.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledAndPublisherCycleHasSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisherCycle.AddBook(book);

            Action act = () => this._publisherCycle.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cycle cannot have duplicated books.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemoveBook()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._publisherCycle.AddBook(book);
            this._publisherCycle.Books.Should().HaveCount(1);
            this._publisherCycle.Books.Should().Contain(book);
            this._publisherCycle.RemoveBook(book);
            this._publisherCycle.Books.Should().HaveCount(0);
            this._publisherCycle.Books.Should().NotContain(book);
        }

        [Test]
        public void RemoveBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._publisherCycle.ArchiveAggregate();

            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._publisherCycle.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndCycleDoesNotHaveSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._publisherCycle.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Publisher cycle does not have selected book.");
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            this._publisherCycle =
                PublisherCycle.Create(new PublisherCycleIdentification(
                    this._fixture.Create<Guid>(),
                    this._fixture.Create<int>()));
        }
    }
}