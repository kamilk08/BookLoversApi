using AutoFixture;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.Services;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class NotificationHiderTests
    {
        private Fixture _fixture;
        private NotificationHider _notificationHider;

        [Test]
        public void ShouldNotificationBeHidden_WhenCalled_NotificationShouldNotBeHidden()
        {
            var option = new HideNewNotification(false);

            var shouldBeHidden = _notificationHider.ShouldNotificationBeHidden(option, NotificationType.Book);

            shouldBeHidden.Should().BeFalse();
        }

        [Test]
        public void ShouldNotificationBeHidden_WhenCalled_NotificationShouldBeHidden()
        {
            var option = new HideNewNotification(true);

            var shouldBeHidden = _notificationHider.ShouldNotificationBeHidden(option, NotificationType.Book);

            shouldBeHidden.Should().BeTrue();
        }

        [Test]
        public void
            ShouldNotificationBeHidden_WhenCalledAndNotificationsAboutBook_ShouldNotBeHidden()
        {
            var bookOption = new HideNotificationAboutBook(false);

            var shouldBeHidden = _notificationHider.ShouldNotificationBeHidden(bookOption, NotificationType.Book);
            shouldBeHidden.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _notificationHider = new NotificationHider();
        }
    }
}