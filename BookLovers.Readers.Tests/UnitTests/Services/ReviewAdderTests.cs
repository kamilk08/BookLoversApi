using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Events.Readers;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class ReviewAdderTests
    {
        private Fixture _fixture;
        private ReviewResourceAdder _resourceAdder;
        private Reader _reader;
        private ReaderFactory _readerFactory;

        [Test]
        public void AddResource_WhenCalled_ShouldAddAuthorAsReaderResource()
        {
            var reviewGuid = _fixture.Create<Guid>();
            var bookGuid = _fixture.Create<Guid>();
            var addedAt = _fixture.Create<DateTime>();

            _resourceAdder.AddResource(_reader, new ReaderReview(reviewGuid, bookGuid, addedAt));

            var @events = _reader.GetUncommittedEvents();
            @events.Should().ContainSingle(p => p.GetType() == typeof(ReaderAddedReview));
            @events.Count().Should().Be(1);
        }

        [Test]
        public void AddResource_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _reader.ApplyChange(new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid));

            _reader.CommitEvents();

            var reviewGuid = _fixture.Create<Guid>();
            var bookGuid = _fixture.Create<Guid>();
            var addedAt = _fixture.Create<DateTime>();

            Action act = () => _resourceAdder.AddResource(_reader, new ReaderReview(reviewGuid, bookGuid, addedAt));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddResource_WhenCalledAndSameResourceAlreadyExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var reviewGuid = _fixture.Create<Guid>();
            var bookGuid = _fixture.Create<Guid>();
            var addedAt = _fixture.Create<DateTime>();

            _resourceAdder.AddResource(_reader, new ReaderReview(reviewGuid, bookGuid, addedAt));

            Action act = () => _resourceAdder.AddResource(_reader, new ReaderReview(reviewGuid, bookGuid, addedAt));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Resource already added.");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _resourceAdder = new ReviewResourceAdder();

            _readerFactory = new ReaderFactory();

            var readerGuid = _fixture.Create<Guid>();
            var profileGuid = _fixture.Create<Guid>();
            var notificationWallGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();
            var userName = _fixture.Create<string>();
            var email = _fixture.Create<string>();

            var readerIdentification = new ReaderIdentification(readerId, userName, email);
            var readerSocials = new ReaderSocials(profileGuid, notificationWallGuid, _fixture.Create<Guid>());

            _reader = _readerFactory.Create(readerGuid, readerIdentification, readerSocials);

            _reader.CommitEvents();
        }
    }
}