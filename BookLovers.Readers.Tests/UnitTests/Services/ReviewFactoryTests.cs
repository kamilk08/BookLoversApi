using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Domain.Reviews.Services;
using BookLovers.Readers.Events.Reviews;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class ReviewFactoryTests
    {
        private ReviewFactory _factory;
        private ReviewParts _reviewParts;
        private Fixture _fixture;

        [Test]
        public void Create_WhenCalled_ShouldCreateNewReview()
        {
            Review review = null;

            Action act = () => review = _factory.Create(_reviewParts);

            act.Should().NotThrow<BusinessRuleNotMetException>();

            var @events = review.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ReviewCreated>();

            review.Guid.Should().Be(_reviewParts.ReviewGuid);
            review.ReviewIdentification.BookGuid.Should().Be(_reviewParts.BookGuid);
            review.ReviewIdentification.ReaderGuid.Should().Be(_reviewParts.ReaderGuid);
            review.ReviewContent.Review.Should().Be(_reviewParts.Content);
            review.ReviewContent.ReviewDate.Should().Be(_reviewParts.ReviewDate);
            review.ReviewContent.EditedDate.Should().Be(null);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _factory = new ReviewFactory();

            _reviewParts = ReviewParts.Initialize()
                .WithGuid(_fixture.Create<Guid>())
                .WitBook(_fixture.Create<Guid>())
                .AddedBy(_fixture.Create<Guid>())
                .HasSpoilers()
                .WithContent(_fixture.Create<string>())
                .WithDates(_fixture.Create<DateTime>(), _fixture.Create<DateTime>());
        }
    }
}