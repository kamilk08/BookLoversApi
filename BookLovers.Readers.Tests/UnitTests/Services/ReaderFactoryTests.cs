using System;
using System.Linq;
using AutoFixture;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Events.TimeLine;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class ReaderFactoryTests
    {
        private ReaderFactory _factory;
        private Fixture _fixture;

        [Test]
        public void Create_WhenCalled_ShouldCreateNewReader()
        {
            var readerGuid = _fixture.Create<Guid>();
            var profileGuid = _fixture.Create<Guid>();
            var notificationWallGuid = _fixture.Create<Guid>();
            var num = _fixture.Create<int>();
            var str = _fixture.Create<string>();
            var email = _fixture.Create<string>();
            var identification = new ReaderIdentification(num, str, email);

            Reader reader = null;

            reader = _factory.Create(readerGuid, identification,
                new ReaderSocials(profileGuid, notificationWallGuid, _fixture.Create<Guid>()));

            reader.Should().NotBeNull();
            reader.Identification.Username.Should().BeEquivalentTo(str);
            reader.Identification.ReaderId.Should().Be(num);
            reader.Socials.ProfileGuid.Should().Be(profileGuid);
            reader.Guid.Should().Be(readerGuid);

            var uncommittedEvents = reader.GetUncommittedEvents().ToList();
            uncommittedEvents.Should().HaveCount(2);
            uncommittedEvents.First().Should().BeOfType<ReaderCreated>();
            uncommittedEvents.Skip(1).First().Should().BeOfType<TimeLineAddedToReader>();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _factory = new ReaderFactory();
        }
    }
}