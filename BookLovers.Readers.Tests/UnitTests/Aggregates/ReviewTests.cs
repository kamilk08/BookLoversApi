using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Shared.Likes;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class ReviewTests
    {
        private Review _review;
        private Fixture _fixture;

        [Test]
        [TestCase("SOME_REVIEW_EDIT", "2014-12-12", true)]
        [TestCase("ABC", "2012-02-02", false)]
        public void EditReview_WhenCalled_ShouldEditReview(string review, DateTime editDate, bool containsSpoilers)
        {
            var readerGuid = _review.ReviewIdentification.ReaderGuid;

            _review.EditReview(readerGuid, new ReviewContent(review, editDate, _review.ReviewContent.ReviewDate),
                containsSpoilers);

            var @events = _review.GetUncommittedEvents();

            _review.ReviewContent.Review.Should().Be(review);
            _review.ReviewContent.EditedDate.Should().Be(editDate);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ReviewEdited>();

            var @event = events.First() as ReviewEdited;
            @event.ContainsSpoilers.Should().Be(containsSpoilers);
        }

        [Test]
        [TestCase("SOME_REVIEW_EDIT", "2014-12-12", true)]
        [TestCase("ABC", "2012-02-02", false)]
        public void EditReview_WhenCalledWithInActiveReview_ShouldThrowBussinesRuleNotMeetException(
            string review,
            DateTime editDate, bool containsSpoilers)
        {
            _review.ApplyChange(new ReviewArchived(_review.Guid, _review.ReviewIdentification.ReaderGuid,
                _review.ReviewIdentification.BookGuid));

            _review.CommitEvents();

            Action act = () => _review.EditReview(
                _review.ReviewIdentification.ReaderGuid,
                new ReviewContent(review, editDate, _review.ReviewContent.ReviewDate),
                containsSpoilers);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        [TestCase("SOME_REVIEW_EDIT", "2014-12-12", true)]
        [TestCase("ABC", "2012-02-02", false)]
        public void
            EditReview_WhenCalledWithReaderGuidDifferentThenOwnerOfReview_ShouldThrowBussinesRuleNotMeetException(
                string review,
                DateTime editDate, bool containsSpoilers)
        {
            Action act = () => _review.EditReview(
                _fixture.Create<Guid>(),
                new ReviewContent(review, editDate, _review.ReviewContent.ReviewDate),
                containsSpoilers);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Only author of review can edit it.");
        }

        [Test]
        public void AddLike_WhenCalled_ShouldAddLikeToReview()
        {
            var readerGuid = _fixture.Create<Guid>();
            var like = Like.NewLike(readerGuid);

            _review.AddLike(like);

            _review.Likes.Should().HaveCount(1);
            _review.Likes.Should().AllBeEquivalentTo(like);

            var @events = _review.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ReviewLiked>();
        }

        [Test]
        public void AddLike_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _review.ApplyChange(new ReviewArchived(_review.Guid, _review.ReviewIdentification.ReaderGuid,
                _review.ReviewIdentification.BookGuid));

            _review.CommitEvents();

            Action act = () => _review.AddLike(Like.NewLike(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddLike_WhenCalledButReaderTriesToLikeOwnReview_ShouldThrowBusinessRuleNotMeetException()
        {
            var like = Like.NewLike(_review.ReviewIdentification.ReaderGuid);

            Action act = () => _review.AddLike(like);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot like own reviews.");
        }

        [Test]
        public void AddLike_WhenCalledButReviewWasLikedAlreadyBySpecificReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var readerGuid = _fixture.Create<Guid>();

            var like = Like.NewLike(readerGuid);

            _review.AddLike(like);

            Action act = () => _review.AddLike(like);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Review cannot have multiple likes from same reader.");
        }

        [Test]
        public void RemoveLike_WhenCalled_ShouldRemoveLike()
        {
            var readerGuid = _fixture.Create<Guid>();

            var like = Like.NewLike(readerGuid);

            _review.AddLike(like);
            _review.CommitEvents();

            _review.RemoveLike(like);

            var @events = _review.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ReviewUnLiked>();

            _review.Likes.Should().HaveCount(0);
            _review.Likes.Should().AllBeEquivalentTo(like);
        }

        [Test]
        public void RemoveLike_WhenCalledWithInActiveManager_ShouldThrowBussinesRuleNotMeetException()
        {
            _review.ApplyChange(new ReviewArchived(_review.Guid, _review.ReviewIdentification.ReaderGuid,
                _review.ReviewIdentification.BookGuid));
            _review.CommitEvents();

            var readerGuid = _fixture.Create<Guid>();

            var like = Like.NewLike(readerGuid);

            Action act = () => _review.RemoveLike(like);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveLike_WhenCalledButThereIsNoLikeFromSelectedReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var like = Like.NewLike(_fixture.Create<Guid>());

            Action act = () => _review.RemoveLike(like);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Review has not been liked by selected reader.");
        }

        [Test]
        public void AddSpoilerTag_WhenCalled_ShouldAddSpoilerTagToReview()
        {
            var userGuid = _fixture.Create<Guid>();

            _review.AddSpoilerTag(new SpoilerTag(userGuid));

            var spolierTag = _review.GetSpoilerTag(userGuid);

            spolierTag.Should().NotBeNull();
        }

        [Test]
        public void AddSpoilerTag_WhenCalledAndReviewHasEnoughSpoilerTagsToMarkItAsaSpoiler_ShouldReviewAsSpoiler()
        {
            for (var i = 0; i < 11; i++)
                _review.AddSpoilerTag(new SpoilerTag(_fixture.Create<Guid>()));

            _review.AddSpoilerTag(new SpoilerTag(_fixture.Create<Guid>()));

            _review.ReviewSpoiler.MarkedByOtherReaders.Should().BeTrue();
        }

        [Test]
        public void RemoveSpoilerTag_WhenCalled_ShouldRemoveSpoilerTag()
        {
            var userGuid = _fixture.Create<Guid>();

            _review.AddSpoilerTag(new SpoilerTag(userGuid));

            var spoilerTag = _review.GetSpoilerTag(userGuid);

            _review.RemoveSpoilerTag(spoilerTag);

            _review.SpoilerTags.Count.Should().Be(0);
        }

        [Test]
        public void GetLike_WhenCalled_ShouldReturnLike()
        {
            var readerGuid = _fixture.Create<Guid>();

            var newLike = Like.NewLike(readerGuid);

            _review.AddLike(newLike);
            _review.CommitEvents();

            var like = _review.GetLike(readerGuid);

            like.Should().NotBeNull();
            like.Should().BeEquivalentTo(newLike);
        }

        [Test]
        public void GetLike_WhenCalledWhenLikeDoesNotExists_ShouldReturnNull()
        {
            var readerGuid = _fixture.Create<Guid>();

            var newLike = Like.NewLike(readerGuid);

            _review.AddLike(newLike);
            _review.CommitEvents();

            var like = _review.GetLike(_fixture.Create<Guid>());

            like.Should().BeNull();
        }

        [Test]
        public void IsLikedBy_WhenCalled_ShouldReturnTrue()
        {
            var readerGuid = _fixture.Create<Guid>();

            var newLike = Like.NewLike(readerGuid);

            _review.AddLike(newLike);
            _review.CommitEvents();

            var result = _review.IsLikedBy(readerGuid);

            result.Should().BeTrue();
        }

        [Test]
        public void IsLikedBy_WhenCalledWithLikeThatDoesNotExists_ShouldReturnFalse()
        {
            var readerGuid = _fixture.Create<Guid>();

            var newLike = Like.NewLike(readerGuid);

            _review.AddLike(newLike);
            _review.CommitEvents();

            var result = _review.IsLikedBy(_fixture.Create<Guid>());

            result.Should().BeFalse();
        }

        [Test]
        public void Report_WhenCalled_ShouldReportReview()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            var @events = _review.GetUncommittedEvents();

            _review.Reports.Should().HaveCount(1);
            _review.Reports.Should().AllBeEquivalentTo(reviewReport);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ReviewReported>();
        }

        [Test]
        public void Report_WhenCalledWithInActiveManager_ShouldThrowBusinessRuleNotMeetException()
        {
            _review.ApplyChange(new ReviewArchived(_review.Guid, _review.ReviewIdentification.ReaderGuid,
                _review.ReviewIdentification.BookGuid));
            _review.CommitEvents();

            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            Action act = () => _review.Report(reviewReport);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void
            Report_WhenCalledAndReviewBeenAlreadyReportedBySelectedReader_ShouldThrowBussinesRuleNotMeetException()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            Action act = () => _review.Report(reviewReport);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Review has been already reported by reader.");
        }

        [Test]
        public void GetReviewReport_WhenCalled_ShouldReturnReviewReport()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            var result = _review.GetReviewReport(reviewReport.ReportedBy);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(reviewReport);
        }

        [Test]
        public void GetReviewReport_WhenCalledWithReportThatNotExists_ShouldReturnNull()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            var result = _review.GetReviewReport(_fixture.Create<Guid>());

            result.Should().BeNull();
        }

        [Test]
        public void HasBeenReportedBy_WhenCalled_ShouldReturnTrue()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            var result = _review.HasBeenReportedBy(readerGuid);

            result.Should().BeTrue();
        }

        [Test]
        public void HasBeenReportedBy_WhenCalledAndReportDoesNotExists_ShouldReturnFalse()
        {
            var readerGuid = _fixture.Create<Guid>();
            var reviewReport = new ReviewReport(readerGuid);

            _review.Report(reviewReport);

            var result = _review.HasBeenReportedBy(_fixture.Create<Guid>());

            result.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            var reviewGuid = _fixture.Create<Guid>();
            var bookGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();

            var reviewContent =
                new ReviewContent(_fixture.Create<string>(), default(DateTime), _fixture.Create<DateTime>());

            _review = new Review(reviewGuid, new ReviewIdentification(bookGuid, readerGuid), reviewContent,
                new ReviewSpoiler(false, false));

            _review.CommitEvents();
        }
    }
}