using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Events.NotificationWalls;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class NotificationWallTests
    {
        private NotificationWall _notificationWall;
        private Fixture _fixture;

        [Test]
        public void AddNotification_WhenCalled_ShouldAddNewNotification()
        {
            var notification = Notification.Create(NotificationSubType.NewFollower, VisibleOnWall.Yes());

            _notificationWall.AddNotification(notification);

            _notificationWall.Notifications.Should().HaveCount(1);
            _notificationWall.Notifications.Should().Contain(notification);
        }

        [Test]
        public void AddNotification_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _notificationWall.ApplyChange(new NotificationWallArchived(_notificationWall.Guid));

            var notification = Notification.Create(NotificationSubType.LostFollower, VisibleOnWall.Yes());

            Action act = () => _notificationWall.AddNotification(notification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void
            AddNotification_WhenCalledAndAggregateHasAlreadyExactSameNotification_ShouldThrowBusinessRuleNotMeetException()
        {
            var notification = Notification.Create(NotificationSubType.LostFollower, VisibleOnWall.Yes());

            _notificationWall.AddNotification(notification);

            var sameNotification = _notificationWall.GetNotification(notification.Guid);

            Action act = () => _notificationWall.AddNotification(sameNotification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void HideNotification_WhenCalled_NotificationShouldBeHidden()
        {
            var notification = Notification.Create(NotificationSubType.LostFollower, VisibleOnWall.Yes());
            _notificationWall.AddNotification(notification);

            _notificationWall.HideNotification(notification);

            var hiddenNotification = _notificationWall.GetNotification(notification.Guid);

            hiddenNotification.IsVisibleOnWall().Should().BeFalse();
        }

        [Test]
        public void HideNotification_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var notification = Notification.Create(NotificationSubType.LostFollower, VisibleOnWall.Yes());
            _notificationWall.AddNotification(notification);

            _notificationWall.ApplyChange(new NotificationWallArchived(_notificationWall.Guid));

            Action act = () => _notificationWall.HideNotification(notification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void HideNotification_WhenCalledAndNotificationIsAlreadyHidden_ShouldThrowBusinessRuleNotMeetException()
        {
            var notification = Notification.Create(NotificationSubType.ReviewReported, VisibleOnWall.Yes());

            _notificationWall.AddNotification(notification);
            _notificationWall.HideNotification(notification);

            var hiddenNotification = _notificationWall.GetNotification(notification.Guid);

            Action act = () => _notificationWall.HideNotification(hiddenNotification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ShowNotification_WhenCalled_ShouldShowHiddenNotification()
        {
            var notification = Notification.Create(NotificationSubType.BookAcceptedByLibrarian, VisibleOnWall.No());

            _notificationWall.AddNotification(notification);
            _notificationWall.ShowNotification(notification);

            var hiddenNotification = _notificationWall.GetNotification(notification.Guid);

            hiddenNotification.IsVisibleOnWall().Should().BeTrue();
        }

        [Test]
        public void ShowNotification_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var notification = Notification.Create(NotificationSubType.NewFollower, VisibleOnWall.Yes());

            _notificationWall.ApplyChange(new NotificationWallArchived(_notificationWall.Guid));

            Action act = () => _notificationWall.ShowNotification(notification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ShowNotification_WhenCalledAndNotificationIsAlreadyVisible_ShouldThrowBusinessRuleNotMeetException()
        {
            var notification = Notification.Create(NotificationSubType.BookDismissedByLibrarian, VisibleOnWall.Yes());

            _notificationWall.AddNotification(notification);

            Action act = () => _notificationWall.ShowNotification(notification);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void GetNotification_WhenCalled_ShouldReturnSelectedNotification()
        {
            var notification = Notification.Create(NotificationSubType.NewFollower, VisibleOnWall.Yes());

            _notificationWall.AddNotification(notification);

            var returnedNotification = _notificationWall.GetNotification(notification.Guid);

            returnedNotification.Should().NotBeNull();
            returnedNotification.Should().BeEquivalentTo(notification);
        }

        [Test]
        public void GetWallOption_WhenCalled_ShouldReturnSelectedOption()
        {
            var wallOption = _notificationWall.GetWallOption(WallOptionType.HideNotificationAboutBook);

            wallOption.Should().NotBeNull();
            wallOption.Option.Should().Be(WallOptionType.HideNotificationAboutBook);
        }

        [Test]
        public void GetAllNotSeenNotifications_WhenCalled_ShouldReturnAllNotSeenNotifications()
        {
            var firstNotification = Notification.Create(NotificationSubType.ReviewReported, VisibleOnWall.No());
            var secondNotification =
                Notification.Create(NotificationSubType.BookAcceptedByLibrarian, VisibleOnWall.No());

            _notificationWall.AddNotification(firstNotification);
            _notificationWall.AddNotification(secondNotification);

            var notSeenNotifications = _notificationWall.GetAllNotSeenNotifications().ToList();

            notSeenNotifications.Should().HaveCount(2);
            notSeenNotifications.Should().AllBeOfType<Notification>();
            notSeenNotifications.Select(s => s.SeenByReader()).Should().AllBeEquivalentTo(false);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _notificationWall = new NotificationWall(
                _fixture.Create<Guid>(),
                _fixture.Create<Guid>());
        }
    }
}