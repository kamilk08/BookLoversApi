using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Domain.RatingStars;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.UnitTests
{
    [TestFixture]
    public class AuthorTests
    {
        private Fixture _fixture;
        private Author _author;

        [Test]
        public void AddBook_WhenCalled_ShouldAddNewBookToAuthor()
        {
            var authorList = new List<Author>() { this._author };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._author.AddBook(book);

            this._author.Should().NotBeNull();
            this._author.Books.Should().HaveCount(1);
        }

        [Test]
        public void AddBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._author.ArchiveAggregate();

            var authorList = new List<Author>() { this._author };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._author.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledAndAuthorHaveBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>() { this._author };
            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._author.AddBook(book);

            Action act = () => this._author.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author cannot have duplicated books.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemoveBookFromAuthor()
        {
            var authorList = new List<Author>() { this._author };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._author.AddBook(book);
            this._author.RemoveBook(book);

            this._author.Books.Should().HaveCount(0);
            this._author.Books.Should().NotContain(book);
        }

        [Test]
        public void RemoveBook_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._author.ArchiveAggregate();
            var authorList = new List<Author>() { this._author };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._author.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndAuthorDoesNotHaveBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>() { this._author };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._author.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author does not have selected book.");
        }

        [Test]
        public void Average_WhenCalled_ShouldReturnAuthorsAverage()
        {
            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>() { this._author };

            var firstBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, firstRatingGiver.ReaderId,
                Star.Five.Value));
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, secondRatingGiver.ReaderId,
                Star.Three.Value));

            var secondBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            secondBook.AddRating(new Rating(firstBook.Identification.BookId, firstRatingGiver.ReaderId,
                Star.One.Value));
            secondBook.AddRating(
                new Rating(firstBook.Identification.BookId, secondRatingGiver.ReaderId, Star.Two.Value));

            this._author.AddBook(firstBook);
            this._author.AddBook(secondBook);

            var expected = new List<Star>()
            {
                Star.Five,
                Star.Three,
                Star.One,
                Star.Two
            }.Average(a => a.Value);

            var actualValue = this._author.Average();

            actualValue.Should().BePositive();
            actualValue.Should().Be(expected);
        }

        [Test]
        public void Average_WhenCalledAndAuthorHasZeroBooks_ShouldReturnAverageEqualToZero()
        {
            this._author.Average().Should().Be(0.0);
        }

        [Test]
        public void RatingsCount_WhenCalled_ShouldReturnAuthorRatingsCount()
        {
            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>() { this._author };

            var firstBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, firstRatingGiver.ReaderId,
                Star.Five.Value));
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, secondRatingGiver.ReaderId,
                Star.Three.Value));

            var secondBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            secondBook.AddRating(new Rating(firstBook.Identification.BookId, firstRatingGiver.ReaderId,
                Star.One.Value));
            secondBook.AddRating(
                new Rating(firstBook.Identification.BookId, secondRatingGiver.ReaderId, Star.Two.Value));

            this._author.AddBook(firstBook);
            this._author.AddBook(secondBook);

            var count = new List<Star>()
            {
                Star.Five,
                Star.Three,
                Star.One,
                Star.Two
            }.Count;

            var actualValue = this._author.RatingsCount();

            actualValue.Should().BePositive();
            actualValue.Should().Be(count);
        }

        [Test]
        public void RatingsCount_WhenCalledAndAuthorBooksDontHaveAnyRatings_ShouldReturnCountEqualToZero()
        {
            this._author.RatingsCount().Should().Be(0);
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
            this._author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));
        }
    }
}