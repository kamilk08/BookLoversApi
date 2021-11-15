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
    public class BookAdderTests
    {
        private BookResourceAdder _bookAdder;
        private Reader _reader;
        private ReaderFactory _readerFactory;
        private Fixture _fixture;

        [Test]
        public void AddResource_WhenCalled_ShouldAddAuthorAsReaderResource()
        {
            var bookGuid = _fixture.Create<Guid>();
            var bookId = _fixture.Create<int>();
            var addedAt = _fixture.Create<DateTime>();

            _bookAdder.AddResource(_reader, new AddedBook(bookGuid, bookId, addedAt));

            var @events = _reader.GetUncommittedEvents();

            @events.Should().Contain(p => p.GetType() == typeof(ReaderAddedBook));
            @events.Count().Should().Be(1);
        }

        [Test]
        public void AddResource_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _reader.ApplyChange(new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid));

            _reader.CommitEvents();

            var bookGuid = _fixture.Create<Guid>();
            var bookId = _fixture.Create<int>();
            var addedAt = _fixture.Create<DateTime>();

            Action act = () => _bookAdder.AddResource(_reader, new AddedBook(bookGuid, bookId, addedAt));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddResource_WhenCalledAndSameResourceAlreadyExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();
            var bookId = _fixture.Create<int>();
            var addedAt = _fixture.Create<DateTime>();

            _bookAdder.AddResource(_reader, new AddedBook(bookGuid, bookId, addedAt));

            Action act = () => _bookAdder.AddResource(_reader, new AddedBook(bookGuid, bookId, addedAt));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Resource already added.");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _bookAdder = new BookResourceAdder();

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