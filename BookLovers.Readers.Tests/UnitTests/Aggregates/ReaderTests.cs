using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Events.TimeLine;
using BookLovers.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class ReaderTests
    {
        private Reader _reader;
        private Fixture _fixture;

        [Test]
        public void ChangeEmail_WhenCalled_ShouldChangeReaderEmail()
        {
            var nextEmail = this._fixture.Create<string>();

            _reader.ChangeEmail(nextEmail);

            _reader.Identification.Email.Should().Be(nextEmail);
        }

        [Test]
        public void AddFollower_WhenCalled_ShouldAddNewFollower()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.AddFollower(follower);

            var @events = _reader.GetUncommittedEvents();

            _reader.Followers.Should().HaveCount(1);
            _reader.Followers.Should().AllBeEquivalentTo(follower);

            @events.Should().HaveCount(2);
        }

        [Test]
        public void AddFollower_WhenCalledWithInActiveManager_ShouldThrowBussinesRuleNotMeetException()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.ApplyChange(new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid));

            _reader.CommitEvents();

            Action act = () => _reader.AddFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddFollower_WhenCalledAndReaderTriesToFollowHimself_ShouldThrowBusinessRuleNotMeetException()
        {
            var followerGuid = _reader.Guid;
            var follower = new Follower(followerGuid);

            Action act = () => _reader.AddFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot follow himself.");
        }

        [Test]
        public void
            AddFollower_WhenCalledWithFollowerThatAlreadyFollowerReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.AddFollower(follower);

            Action act = () => _reader.AddFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot have duplicated followers.");
        }

        [Test]
        public void RemoveFollower_WhenCalled_ShouldRemoveFollower()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.AddFollower(follower);

            _reader.CommitEvents();

            _reader.RemoveFollower(follower);

            var @events = _reader.GetUncommittedEvents();

            _reader.Followers.Should().HaveCount(0);
            @events.Should().HaveCount(2);
        }

        [Test]
        public void RemoveFollower_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.ApplyChange(new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid));

            Action act = () => _reader.RemoveFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        public void RemoveFollower_WhenCalledWithMissingFollower_ShouldThrowBusinessRuleNotMeetException()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            Action act = () => _reader.RemoveFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader does not have selected follower.");
        }

        [Test]
        public void GetFollower_WhenCalled_ShouldReturnSelectedFollower()
        {
            var followerGuid = _fixture.Create<Guid>();
            var follower = new Follower(followerGuid);

            _reader.AddFollower(follower);

            var result = _reader.GetFollower(follower.FollowedBy);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(follower);
        }

        public void GetFollower_WhenCalledWithFollowerThatDoesNotExists_ShouldReturnNull()
        {
            var result = _reader.GetFollower(_fixture.Create<Guid>());

            result.Should().BeNull();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AddToTimeLine_WhenCalled_ShouldAddActivityToReaderTimeLine(byte activityType)
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var objectGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, Activities.GetActivity(activityType));
            var activity = Activity.InitiallyPublic(objectGuid, activityContent);

            _reader.AddToTimeLine(activity);

            var @events = _reader.GetUncommittedEvents();

            _reader.TimeLine.TimeLineActivities.Should().HaveCount(1);
            _reader.TimeLine.TimeLineActivities.Should().AllBeOfType<Activity>();
            _reader.TimeLine.TimeLineActivities.Should().AllBeEquivalentTo(activity);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ActivityAddedToTimeLine>();
        }

        [Test]
        public void AddToTimeLine_WhenCalledWithInActiveReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var bookGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedBook);
            var activity = Activity.InitiallyPublic(bookGuid, activityContent);

            var @event = new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid);

            _reader.ApplyChange(@event);

            Action act = () => _reader.AddToTimeLine(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        [TestCase(0)]
        [TestCase(255)]
        [TestCase(100)]
        [TestCase(25)]
        public void AddToTimeLine_WhenCalledWithUnknownActivityType_ShouldThrowUnknownActivityTypeException(
            byte activityType)
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var objectGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, Activities.GetActivity(activityType));
            var activity = Activity.InitiallyPublic(objectGuid, activityContent);

            Action act = () => _reader.AddToTimeLine(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Invalid activity type.");
        }

        [Test]
        public void AddToTimeLine_WhenCalledWithActivityThatAlreadyExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var authorGuid = _fixture.Create<Guid>();
            var title = _fixture.Create<string>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);
            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            _reader.AddToTimeLine(activity);

            Action act = () => _reader.AddToTimeLine(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Acvity is already on timeline.");
        }

        [Test]
        public void HideActivity_WhenCalled_ShouldHideActivityFromOthers()
        {
            var activityObjectGuid = _fixture.Create<Guid>();
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();

            var @event = ActivityAddedToTimeLine.Initialize()
                .WithAggregate(_reader.Guid)
                .WithTimeLine(_reader.TimeLine.Guid)
                .WithActivityObject(activityObjectGuid)
                .WithActivity(title, date, ActivityType.AddedAuthor.Value);

            _reader.ApplyChange(@event);
            _reader.CommitEvents();

            var activityToHide = _reader.TimeLine.GetActivity(activityObjectGuid, date, @event.ActivityType);
            _reader.HideActivity(activityToHide);

            var @events = _reader.GetUncommittedEvents();
            var activity = _reader.TimeLine.GetActivity(activityObjectGuid, date, @event.ActivityType);

            activity.Should().NotBeNull();
            activity.ShowToOthers.Should().BeFalse();

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ActivityHiddenOnTimeLine>();
        }

        [Test]
        public void HideActivity_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            var @event = new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid);

            _reader.ApplyChange(@event);

            Action act = () => _reader.HideActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void HideActivity_WhenCalledAndActivityIsNotVisible_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            _reader.AddToTimeLine(activity);

            _reader.HideActivity(activity);

            activity = _reader.TimeLine.GetActivity(authorGuid, date, activity.Content.ActivityType.Value);

            Action act = () => _reader.HideActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Activity must be visible to hide it.");
        }

        [Test]
        [TestCase(125)]
        [TestCase(255)]
        public void HideActivity_WhenCalledWithInvalidActivityType_ShouldThrowBusinessRuleNotMeetException(
            byte activityType)
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();

            var activityContent = new ActivityContent(title, date, Activities.GetActivity(activityType));
            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            var @event = ActivityAddedToTimeLine.Initialize()
                .WithAggregate(this._reader.Guid)
                .WithTimeLine(this._reader.TimeLine.Guid)
                .WithActivityObject(authorGuid)
                .WithActivity(title, date, activityType);

            _reader.ApplyChange(@event);

            Action act = () => _reader.HideActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void HideActivity_WhenCalledAndActivityIsMissing_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();

            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);
            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            Action act = () => _reader.HideActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ShowActivity_WhenCalled_ShouldShowActivityToOthers()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            _reader.AddToTimeLine(activity);
            _reader.HideActivity(activity);
            _reader.CommitEvents();

            activity = _reader.TimeLine.GetActivity(authorGuid, date, activity.Content.ActivityType.Value);

            _reader.ShowActivity(activity);

            var @events = _reader.GetUncommittedEvents();
            var activityToShow = _reader.TimeLine.GetActivity(authorGuid, date, activity.Content.ActivityType.Value);

            _reader.TimeLine.TimeLineActivities.Should().HaveCount(1);
            _reader.TimeLine.TimeLineActivities.Should().AllBeOfType<Activity>();
            activityToShow.Should().NotBeNull();
            activityToShow.ShowToOthers.Should().BeTrue();

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ActivityShownOnTimeLine>();
        }

        [Test]
        public void ShowActivity_WhenCalledWithInActiveReader_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            var @event = new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid);

            _reader.ApplyChange(@event);

            Action act = () => _reader.ShowActivity(activity);
            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ShowActivity_WhenCalledButActivityIsVisible_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            _reader.AddToTimeLine(activity);

            Action act = () => _reader.ShowActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Activity already vissible.");
        }

        [Test]
        public void ShowActivity_WhenCalledButActivityIsMissing_ShouldThrowBusinessRuleNotMeetException()
        {
            var date = _fixture.Create<DateTime>();
            var title = _fixture.Create<string>();
            var authorGuid = _fixture.Create<Guid>();
            var activityContent = new ActivityContent(title, date, ActivityType.AddedAuthor);

            var activity = Activity.InitiallyPublic(authorGuid, activityContent);

            Action act = () => _reader.ShowActivity(activity);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Activity missing on timeline.");
        }

        [Test]
        public void RemoveAddedResource_WhenCalled_ShouldRemoveAddedByReaderResource()
        {
            var bookGuid = _fixture.Create<Guid>();
            var bookId = _fixture.Create<int>();
            var addedAt = _fixture.Create<DateTime>();

            var @event = ReaderAddedBook.Initialize()
                .WithAggregate(_reader.Guid)
                .WithBook(bookGuid, bookId)
                .WithAddedAt(addedAt);

            _reader.ApplyChange(@event);
            _reader.CommitEvents();

            _reader.RemoveAddedResource(bookGuid);

            var @events = _reader.GetUncommittedEvents();
            @events.Should().ContainSingle(p => p.GetType() == typeof(AddedResourceRemoved));

            _reader.AddedResources.Count.Should().Be(0);
        }

        [Test]
        public void RemoveAddedResource_WhenCalledAndReaderIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _reader.ApplyChange(new ReaderSuspended(_reader.Guid, _reader.Socials.ProfileGuid,
                _reader.Socials.NotificationWallGuid));
            _reader.CommitEvents();

            Action act = () => _reader.RemoveAddedResource(_fixture.Create<Guid>());

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveAddedResource_WhenCalledAndResourceIsNotPresent_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => _reader.RemoveAddedResource(_fixture.Create<Guid>());

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Added resource is not available.");
        }

        [Test]
        public void GetResource_WhenCalled_ShouldReturnAddedResource()
        {
            var bookGuid = _fixture.Create<Guid>();
            var bookId = _fixture.Create<int>();
            var addedAt = _fixture.Create<DateTime>();

            var @event = ReaderAddedBook.Initialize()
                .WithAggregate(_reader.Guid)
                .WithBook(bookGuid, bookId)
                .WithAddedAt(addedAt);

            _reader.ApplyChange(@event);
            _reader.CommitEvents();

            var result = _reader.GetResource(bookGuid, AddedResourceType.Book);

            result.Should().NotBeNull();
            result.ResourceGuid.Should().Be(bookGuid);
        }

        [Test]
        public void GetResource_WhenCalled_ShouldReturnNull()
        {
            var result = _reader.GetResource(_fixture.Create<Guid>(), AddedResourceType.Book);

            result.Should().BeNull();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            var readerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();
            var profileGuid = _fixture.Create<Guid>();
            var userName = _fixture.Create<string>();
            var notificationWallGuid = _fixture.Create<Guid>();
            var email = _fixture.Create<string>();

            var readerIdentification = new ReaderIdentification(readerId, userName, email);
            var readerSocials = new ReaderSocials(profileGuid, notificationWallGuid, _fixture.Create<Guid>());

            _reader = new Reader(readerGuid, readerIdentification, readerSocials);

            _reader.CommitEvents();
        }
    }
}