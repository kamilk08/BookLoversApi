using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Mementos
{
    public class NotificationWallMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private NotificationWall _notificationWall;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_notificationWall);
            var notificationWall = (NotificationWall) ReflectionHelper.CreateInstance(typeof(NotificationWall));
            var memento = _mementoFactoryMock.Object.Create<NotificationWall>();

            memento = (IMemento<NotificationWall>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            notificationWall.ApplySnapshot(memento);
            notificationWall.Should().NotBeNull();
            notificationWall.Should().NotBe(null);
            notificationWall.Should().BeEquivalentTo(_notificationWall);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_notificationWall);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_notificationWall.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<NotificationWall>())
                .Returns(new NotificationWallMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
            _notificationWall = new NotificationWall(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            Enumerable.Range(0, 20).ForEach(i =>
            {
                _notificationWall.AddNotification(Notification.Create(
                    NotificationSubType.NewFollower,
                    VisibleOnWall.Yes()));
                _notificationWall.AddNotification(Notification.Create(
                    NotificationSubType.ReviewReported,
                    VisibleOnWall.No()));
                _notificationWall.AddNotification(Notification.Create(
                    NotificationSubType.BookAcceptedByLibrarian,
                    VisibleOnWall.Yes()));
            });
        }
    }
}