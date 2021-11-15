using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Mementos
{
    [TestFixture]
    public class ReaderMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Reader _reader;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_reader);
            var reader = (Reader) ReflectionHelper.CreateInstance(typeof(Reader));
            var memento = _mementoFactoryMock.Object.Create<Reader>();
            memento = (IMemento<Reader>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            reader.ApplySnapshot(memento);
            reader.Should().NotBeNull();
            reader.Should().BeEquivalentTo(_reader);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_reader);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_reader.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Reader>())
                .Returns(new ReaderMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _reader = new Reader(
                _fixture.Create<Guid>(),
                new ReaderIdentification(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<string>()),
                new ReaderSocials(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<Guid>()));

            AddDataToReader();
        }

        private void AddDataToReader()
        {
            for (var index = 0; index < 20; ++index)
                _reader.AddFollower(new Follower(_fixture.Create<Guid>()));

            _reader.ChangeEmail(_fixture.Create<string>());

            _reader.AddToTimeLine(Activity.InitiallyPublic(
                _fixture.Create<Guid>(),
                new ActivityContent(_fixture.Create<string>(), _fixture.Create<DateTime>(), ActivityType.AddedAuthor)));

            _reader.ApplyChange(ReaderAddedBook.Initialize().WithAggregate(_reader.Guid)
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<int>()).WithAddedAt(_fixture.Create<DateTime>()));

            _reader.ApplyChange(ReaderAddedAuthor.Initialize().WithAggregate(_reader.Guid)
                .WithAuthor(_fixture.Create<Guid>(), _fixture.Create<int>()).WithAddedAt(_fixture.Create<DateTime>()));
        }
    }
}