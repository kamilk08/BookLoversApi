using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Shared.Likes;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class QuotesTests
    {
        private Quote _quote;
        private Fixture _fixture;

        [Test]
        public void Like_WhenCalled_ShouldAddLikeToQuote()
        {
            var like = Like.NewLike(_fixture.Create<Guid>());

            _quote.AddLike(like);

            _quote.Likes.Should().HaveCount(1);
            _quote.Likes.Should().ContainSingle(p => p.ReaderGuid == like.ReaderGuid);

            var @events = _quote.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(QuoteLiked));
        }

        [Test]
        public void Like_WhenCalledAndQuoteIsArchived_ShouldThrowBusinessRuleNotMeetException()
        {
            var objectGuid = _fixture.Create<Guid>();

            var @event = new QuoteArchived(_quote.Guid, objectGuid, _quote.QuoteDetails.AddedByGuid);

            _quote.ApplyChange(@event);
            _quote.CommitEvents();

            Action act = () => _quote.AddLike(Like.NewLike(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void Like_WhenCalledAndQuoteHasBeenAlreadyLikedByCertainReader_ShouldThrowBussinesRuleMeetException()
        {
            var readerGuid = _fixture.Create<Guid>();

            _quote.AddLike(Like.NewLike(readerGuid));

            Action act = () => _quote.AddLike(Like.NewLike(readerGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Quote cannot be liked multiple times by same reader");
        }

        [Test]
        public void UnLike_WhenCalled_ShouldUnlikeQuote()
        {
            var readerGuid = _fixture.Create<Guid>();

            _quote.AddLike(Like.NewLike(readerGuid));
            _quote.CommitEvents();

            var like = _quote.GetLike(readerGuid);

            _quote.UnLike(like);

            _quote.Likes.Should().HaveCount(0);

            var @events = _quote.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(QuoteUnLiked));
        }

        [Test]
        public void UnLike_WhenCalledWithArchivedQuote_ShouldThrowBussinesRuleNotMeetException()
        {
            var objectGuid = _fixture.Create<Guid>();

            var @event = new QuoteArchived(_quote.Guid, objectGuid, _fixture.Create<Guid>());

            _quote.ApplyChange(@event);
            _quote.CommitEvents();

            Action act = () => _quote.UnLike(Like.NewLike(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void UnLike_WhenCalledAndQuoteLikeIsMissing_ShouldThrowBussinesRuleNotMeetException()
        {
            Action act = () => _quote.UnLike(Like.NewLike(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Quote must contain selected like.");
        }

        [Test]
        public void GetLike_WhenCalled_ShouldReturnSelectedLike()
        {
            var readerGuid = _fixture.Create<Guid>();
            var newLike = Like.NewLike(readerGuid);

            _quote.AddLike(newLike);

            var like = _quote.GetLike(readerGuid);

            like.Should().NotBeNull();
            like.Should().BeEquivalentTo(newLike);
        }

        [Test]
        public void GetLike_WhenCalled_ShouldReturnNull()
        {
            var readerGuid = _fixture.Create<Guid>();

            _quote.AddLike(Like.NewLike(readerGuid));

            var like = _quote.GetLike(_fixture.Create<Guid>());

            like.Should().BeNull();
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            var quoteContent = _fixture.Create<string>();

            var bookGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();

            this._quote = new Quote(_fixture.Create<Guid>(), new QuoteContent(quoteContent, bookGuid),
                new QuoteDetails(readerGuid, _fixture.Create<DateTime>(), QuoteType.BookQuote));

            this._quote.CommitEvents();
        }
    }
}