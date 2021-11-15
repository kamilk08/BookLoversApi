using System;
using System.Collections.Generic;
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
    public class BookTests
    {
        private Fixture _fixture;
        private Book _book;

        [Test]
        public void AddRating_WhenCalled_ShouldAddNewRatingToBook()
        {
            var ratingGiver = new RatingGiver(
                this._fixture.Create<Guid>(),
                this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(
                this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value);

            this._book.AddRating(rating);

            this._book.Ratings.Should().HaveCount(1);
            this._book.Ratings.Should().Contain(rating);
        }

        [Test]
        public void AddRating_WhenCalledAndBookIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._book.ArchiveAggregate();

            var ratingGiver = new RatingGiver(
                this._fixture.Create<Guid>(),
                this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(
                this._book.Identification.BookId,
                ratingGiver.ReaderId,
                Star.Five.Value);

            Action act = () => this._book.AddRating(rating);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddRating_WhenCalledAndBookHasAlreadyRatingFromSameReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var readerGuid = this._fixture.Create<Guid>();
            var readerId = this._fixture.Create<int>();

            var rating = new Rating(
                this._book.Identification.BookId,
                new RatingGiver(this._fixture.Create<Guid>(), readerGuid, readerId).ReaderId, Star.Four.Value);

            this._book.AddRating(rating);

            Action act = () => this._book.AddRating(rating);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book cannot have multiple ratings from same reader.");
        }

        [Test]
        public void AddRating_WhenCalledAndRatingIsNotValid_ShouldThrowBusinessRuleNotMeetException()
        {
            var readerGuid = this._fixture.Create<Guid>();
            var readerId = this._fixture.Create<int>();

            var rating = new Rating(
                this._book.Identification.BookId,
                new RatingGiver(this._fixture.Create<Guid>(), readerGuid, readerId).ReaderId, 20);

            Action act = () => this._book.AddRating(rating);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Rating stars is not valid.");
        }

        [Test]
        public void ChangeRating_WhenCalled_ShouldChangeRating()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            this._book.AddRating(new Rating(this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value));

            this._book.ChangeRating(
                ratingGiver,
                new Rating(this._book.Identification.BookId, ratingGiver.ReaderId, Star.Two.Value));

            this._book.Ratings.Should().HaveCount(1);
            this._book.Ratings.Should().ContainSingle(p => p.Stars == Star.Two.Value);
        }

        [Test]
        public void ChangeRating_WhenCalledAndBookDoesNotHaveSelectedRating_ShouldThrowBusinessRuleNotMeetException()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            Action act = () => this._book.ChangeRating(
                ratingGiver,
                new Rating(this._book.Identification.BookId, ratingGiver.ReaderId, 20));

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Book does not have selected rating.");
        }

        [Test]
        public void ChangeRating_WhenCalledAndRatingIsNotValid_ShouldThrowBusinessRuleNotMeetException()
        {
            var reader = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            this._book.AddRating(new Rating(this._book.Identification.BookId, reader.ReaderId, Star.Five.Value));

            Action act = () =>
                this._book.ChangeRating(reader, new Rating(this._book.Identification.BookId, reader.ReaderId, 20));

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Rating stars is not valid.");
        }

        [Test]
        public void RemoveRating_WhenCalled_ShouldRemoveRatingFromBook()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(
                this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value);

            this._book.AddRating(rating);
            this._book.RemoveRating(rating);

            this._book.Ratings.Should().HaveCount(0);
            this._book.Ratings.Should().NotContain(rating);
        }

        [Test]
        public void RemoveRating_WhenCalledAndBookIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            this._book.ArchiveAggregate();

            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(
                this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value);

            Action act = () => this._book.RemoveRating(rating);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveRating_WhenCalledAndRatingDoesNotExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(
                this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value);

            Action act = () => this._book.RemoveRating(rating);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Cannot remove rating that has not been added by reader");
        }

        [Test]
        public void GetReaderRating_WhenCalled_ShouldReturnReaderRating()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            var rating = new Rating(this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value);

            this._book.AddRating(rating);

            var readerRating = this._book.GetReaderRating(ratingGiver.ReaderId);

            readerRating.Should().NotBeNull();
            readerRating.Should().BeEquivalentTo(rating);
        }

        [Test]
        public void GetReaderRating_WhenCalled_ShouldReturnNull()
        {
            this._book.GetReaderRating(this._fixture.Create<int>()).Should().BeNull();
        }

        [Test]
        public void HasRating_WhenCalled_ShouldReturnTrue()
        {
            var ratingGiver = new RatingGiver(this._fixture.Create<Guid>(), this._fixture.Create<Guid>(),
                this._fixture.Create<int>());

            this._book.AddRating(new Rating(this._book.Identification.BookId, ratingGiver.ReaderId, Star.Five.Value));

            this._book.HasRating(ratingGiver.ReaderId).Should().BeTrue();
        }

        [Test]
        public void HasRating_WhenCalled_ShouldReturnFalse()
        {
            this._book.HasRating(this._fixture.Create<int>()).Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            this._book = Book.Create(
                new BookIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()),
                new List<Author>()
                {
                    Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>())),
                    Author.Create(new AuthorIdentification(this._fixture.Create<Guid>(), this._fixture.Create<int>()))
                });
        }
    }
}