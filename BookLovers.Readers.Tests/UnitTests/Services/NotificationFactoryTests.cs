using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.Services;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class NotificationFactoryTests
    {
        private Fixture _fixture;
        private NotificationFactory _notificationFactory;

        [Test]
        public void
            CreateNotification_WhenCalledAndTypeOfNotificationIsNewFollower_ShouldCreateNewFollowerNotification()
        {
            var notificationItems = new List<NotificationItem>()
                { new NotificationItem(_fixture.Create<Guid>(), NotificationItemType.User) };
            var notification = _notificationFactory.CreateNotification(
                notificationItems,
                NotificationSubType.NewFollower, new List<IWallOption>());

            notification.Should().NotBeNull();
            var items = notification.GetNotificationItems();
            items.Count().Should().Be(1);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var hider = new NotificationHider();

            _notificationFactory = new NotificationFactory(hider);
        }
    }
}