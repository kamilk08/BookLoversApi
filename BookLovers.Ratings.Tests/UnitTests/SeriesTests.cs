using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.BookSeries;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Domain.RatingStars;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.UnitTests
{
    [TestFixture]
    public class SeriesTests
    {
        private Fixture _fixture;
        private Series _series;

        [Test]
        public void AddBook_WhenCalled_ShouldAddBookToSeries()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._series.AddBook(book);

            this._series.Books.Should().HaveCount(1);
            this._series.Books.Should().Contain(book);
        }

        [Test]
        public void AddBook_WhenCalledAndSeriesIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._series.ArchiveAggregate();

            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._series.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledAndSeriesAlreadyHasSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._series.AddBook(book);

            Action act = () => this._series.AddBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Series cannot have duplicated books.");
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

            this._series.AddBook(book);

            this._series.RemoveBook(book);

            this._series.Books.Should().HaveCount(0);
            this._series.Books.Should().NotContain(book);
        }

        [Test]
        public void RemoveBook_WhenCalledAndSeriesIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._series.ArchiveAggregate();

            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._series.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndSeriesDoesNotHaveSelectedBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var authorList = new List<Author>()
            {
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
            };

            var book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            Action act = () => this._series.RemoveBook(book);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Series must have selected book.");
        }

        [Test]
        public void Average_WhenCalled_ShouldReturnBookAverage()
        {
            var author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));

            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>()
            {
                author
            };

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

            this._series.AddBook(firstBook);
            this._series.AddBook(secondBook);

            var expected = new List<Star>()
            {
                Star.Five,
                Star.Three,
                Star.One,
                Star.Two
            }.Average(a => a.Value);

            var actualValue = this._series.Average();

            actualValue.Should().BePositive();
            actualValue.Should().Be(expected);
        }

        [Test]
        public void Average_WhenCalledAndSeriesDoesNotHaveBooks_ShouldReturnAverageEqualToZero()
        {
            this._series.Average().Should().Be(0.0);
        }

        [Test]
        public void Average_WhenCalledAndSeriesBooksDontHaveRatings_ShouldReturnAverageEqualToZero()
        {
            var author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));

            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>()
            {
                author
            };
            var firstBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            var secondBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._series.AddBook(firstBook);
            this._series.AddBook(secondBook);

            this._series.Average().Should().Be(0.0);
        }

        [Test]
        public void RatingsCount_WhenCalled_ShouldReturnRatingsCount()
        {
            var author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));

            var ratingGiver1 = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var ratingGiver2 = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>()
            {
                author
            };

            var firstBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, ratingGiver1.ReaderId, Star.Five.Value));
            firstBook.AddRating(new Rating(firstBook.Identification.BookId, ratingGiver2.ReaderId, Star.Three.Value));

            var secondBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            secondBook.AddRating(new Rating(firstBook.Identification.BookId, ratingGiver1.ReaderId, Star.One.Value));
            secondBook.AddRating(new Rating(firstBook.Identification.BookId, ratingGiver2.ReaderId, Star.Two.Value));

            this._series.AddBook(firstBook);
            this._series.AddBook(secondBook);

            var count = new List<Star>()
            {
                Star.Five,
                Star.Three,
                Star.One,
                Star.Two
            }.Count;

            var actualValue = this._series.RatingsCount();
            actualValue.Should().BePositive();
            actualValue.Should().Be(count);
        }

        [Test]
        public void RatingsCount_WhenCalledAndSeriesDoesNotHaveBooks_ShouldReturnCountEqualToZero()
        {
            var author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));

            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            this._series.RatingsCount().Should().Be(0);
        }

        [Test]
        public void RatingsCount_WhenCalledAndSeriesBooksDontHaveRatings_ShouldReturnCountEqualToZero()
        {
            var author =
                Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));

            var firstRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());
            var secondRatingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var authorList = new List<Author>()
            {
                author
            };

            var firstBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);
            var secondBook = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                authorList);

            this._series.AddBook(firstBook);
            this._series.AddBook(secondBook);

            this._series.RatingsCount().Should().Be(0);
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
            this._series =
                Series.Create(new SeriesIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()));
        }
    }
}