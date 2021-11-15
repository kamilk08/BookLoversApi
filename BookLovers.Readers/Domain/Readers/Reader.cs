using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Domain.Readers.BusinessRules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Events.TimeLine;
using BookLovers.Readers.Mementos;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Readers
{
    [AllowSnapshot]
    public class Reader :
        EventSourcedAggregateRoot,
        IHandle<ReaderCreated>,
        IHandle<ReaderEmailChanged>,
        IHandle<ReaderSuspended>,
        IHandle<ReaderAddedBook>,
        IHandle<ReaderAddedAuthor>,
        IHandle<ReaderAddedReview>,
        IHandle<AddedResourceRemoved>,
        IHandle<ReaderFollowed>,
        IHandle<ReaderUnFollowed>,
        IHandle<TimeLineAddedToReader>,
        IHandle<ActivityAddedToTimeLine>,
        IHandle<ActivityShownOnTimeLine>,
        IHandle<ActivityHiddenOnTimeLine>
    {
        private List<IAddedResource> _addedResources = new List<IAddedResource>();
        private List<Follower> _followers = new List<Follower>();

        public ReaderIdentification Identification { get; private set; }

        public ReaderSocials Socials { get; private set; }

        public TimeLine TimeLine { get; private set; }

        public IReadOnlyList<IAddedResource> AddedResources => _addedResources;

        public IReadOnlyList<Follower> Followers => _followers;

        protected Reader()
        {
        }

        public Reader(Guid readerGuid, ReaderIdentification identification, ReaderSocials socials)
        {
            Guid timeLineGuid = Guid.NewGuid();
            Guid = readerGuid;
            Identification = identification;
            Socials = socials;
            AggregateStatus = AggregateStatus.Active;

            var @event = ReaderCreated.Initialize()
                .WithAggregate(Guid)
                .WithReader(identification.ReaderId, identification.Username, identification.Email)
                .WithSocials(socials.ProfileGuid, socials.NotificationWallGuid, socials.StatisticsGathererGuid,
                    timeLineGuid);

            ApplyChange(@event);
            ApplyChange(new TimeLineAddedToReader(Guid, timeLineGuid));
        }

        public void ChangeEmail(string email)
        {
            ApplyChange(new ReaderEmailChanged(Guid, email));
        }

        public void AddFollower(Follower follower)
        {
            CheckBusinessRules(new AddFollowerRules(this, follower));

            ApplyChange(new ReaderFollowed(Guid, Socials.NotificationWallGuid, follower.FollowedBy));

            var activity = Activity.InitiallyPublic(
                follower.FollowedBy,
                new ActivityContent("Reader has new follower", DateTime.UtcNow, ActivityType.NewFollower));

            AddToTimeLine(activity);
        }

        public void RemoveFollower(Follower follower)
        {
            CheckBusinessRules(new RemoveFollowerRules(this, follower));

            ApplyChange(new ReaderUnFollowed(Guid, Socials.NotificationWallGuid, follower.FollowedBy));

            var activity = Activity.InitiallyPublic(
                follower.FollowedBy,
                new ActivityContent("Reader lost follower", DateTime.UtcNow, ActivityType.LostFollower));

            AddToTimeLine(activity);
        }

        public void RemoveAddedResource(Guid resourceGuid)
        {
            CheckBusinessRules(new RemoveReaderResourceRules(
                this,
                _addedResources.SingleOrDefault(p => p.ResourceGuid == resourceGuid)));

            ApplyChange(new AddedResourceRemoved(Guid, resourceGuid));
        }

        public void AddToTimeLine(Activity activity)
        {
            CheckBusinessRules(new AddToTimeLineRules(this, activity));

            var @event = ActivityAddedToTimeLine.Initialize()
                .WithAggregate(Guid)
                .WithTimeLine(TimeLine.Guid)
                .WithActivityObject(activity.TimeLineObjectGuid)
                .WithActivity(
                    activity.Content.Title,
                    activity.Content.Date, activity.Content.ActivityType.Value);

            ApplyChange(@event);
        }

        public void ShowActivity(Activity activity)
        {
            CheckBusinessRules(new ShowActivityRules(this, activity));

            var @event = ActivityShownOnTimeLine.Initialize()
                .WithAggregate(Guid)
                .WithTimeLine(TimeLine.Guid)
                .WithItem(activity.TimeLineObjectGuid, activity.Content.Date)
                .WithType(activity.Content.ActivityType.Value);

            ApplyChange(@event);
        }

        public void HideActivity(Activity activity)
        {
            CheckBusinessRules(new HideActivityRules(this, activity));

            var @event = ActivityHiddenOnTimeLine.Initialize().WithAggregate(Guid).WithTimeLine(TimeLine.Guid)
                .WithItem(activity.TimeLineObjectGuid, activity.Content.Date)
                .WithType(activity.Content.ActivityType.Value);

            ApplyChange(@event);
        }

        public Follower GetFollower(Guid followerGuid)
        {
            return _followers.Find(p => p.FollowedBy == followerGuid);
        }

        public IAddedResource GetResource(
            Guid resourceGuid,
            AddedResourceType addedResourceType)
        {
            return _addedResources.Find(p =>
                p.ResourceGuid == resourceGuid && p.AddedResourceType == addedResourceType);
        }

        public IEnumerable<ReaderReview> GetAllAddedReviews()
        {
            return _addedResources
                .FindAll(p => p.AddedResourceType == AddedResourceType.Review)
                .Cast<ReaderReview>();
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var readerMemento = memento as IReaderMemento;

            Guid = readerMemento.AggregateGuid;
            AggregateStatus = AggregateStates.Get(readerMemento.AggregateStatus);
            LastCommittedVersion = readerMemento.LastCommittedVersion;
            Version = readerMemento.Version;

            Identification =
                new ReaderIdentification(readerMemento.ReaderId, readerMemento.UserName, readerMemento.Email);
            Socials = new ReaderSocials(readerMemento.SocialProfileGuid, readerMemento.NotificationWallGuid,
                readerMemento.StatisticsGathererGuid);

            _addedResources = readerMemento.AddedResources.ToList();
            _followers = readerMemento.Followers.Select(s => new Follower(s)).ToList();

            TimeLine = new TimeLine(readerMemento.TimeLineGuid);
            TimeLine.Activites = readerMemento.Activities.ToList();
        }

        void IHandle<ReaderSuspended>.Handle(ReaderSuspended @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<ReaderCreated>.Handle(ReaderCreated @event)
        {
            Guid = @event.AggregateGuid;
            Identification = new ReaderIdentification(@event.ReaderId, @event.UserName, @event.Email);
            Socials = new ReaderSocials(@event.SocialProfileGuid, @event.NotificationWallGuid,
                @event.StatisticsGathererGuid);
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<ActivityAddedToTimeLine>.Handle(ActivityAddedToTimeLine @event)
        {
            var activity = new Activity(
                @event.ActivityObjectGuid,
                new ActivityContent(@event.Title, @event.Date, Activities.GetActivity(@event.ActivityType)),
                @event.ShowToOthers);

            TimeLine.Activites.Add(activity);
        }

        void IHandle<ActivityShownOnTimeLine>.Handle(
            ActivityShownOnTimeLine @event)
        {
            var activity = TimeLine.GetActivity(@event.TimeLineObjectGuid, @event.OccuredAt, @event.ActivityTypeId);

            TimeLine.Activites.Remove(activity);
            TimeLine.Activites.Add(activity.Show());
        }

        void IHandle<ActivityHiddenOnTimeLine>.Handle(
            ActivityHiddenOnTimeLine @event)
        {
            var activity = TimeLine.GetActivity(@event.TimeLineObjectGuid, @event.OccuredAt, @event.ActivityTypeId);

            TimeLine.Activites.Remove(activity);

            TimeLine.Activites.Add(activity.Hide());
        }

        void IHandle<TimeLineAddedToReader>.Handle(
            TimeLineAddedToReader @event)
        {
            TimeLine = new TimeLine(@event.TimeLineGuid);
        }

        void IHandle<ReaderAddedBook>.Handle(ReaderAddedBook @event)
        {
            _addedResources.Add(new AddedBook(@event.BookGuid, @event.BookId, @event.AddedAt));
        }

        void IHandle<ReaderAddedAuthor>.Handle(ReaderAddedAuthor @event)
        {
            _addedResources.Add(new AddedAuthor(@event.AuthorGuid, @event.AuthorId, @event.AddedAt));
        }

        void IHandle<ReaderAddedReview>.Handle(ReaderAddedReview @event)
        {
            _addedResources.Add(new ReaderReview(@event.ReviewGuid, @event.BookGuid, @event.AddedAt));
        }

        void IHandle<AddedResourceRemoved>.Handle(AddedResourceRemoved @event)
        {
            var resource = _addedResources.Single(p => p.ResourceGuid == @event.ResourceGuid);

            _addedResources.Remove(resource);
        }

        void IHandle<ReaderFollowed>.Handle(ReaderFollowed @event) =>
            _followers.Add(new Follower(@event.FollowedByGuid));

        void IHandle<ReaderUnFollowed>.Handle(ReaderUnFollowed @event)
        {
            var follower = _followers.Find(p => p.FollowedBy == @event.UnFollowedByGuid);

            _followers.Remove(follower);
        }

        void IHandle<ReaderEmailChanged>.Handle(ReaderEmailChanged @event)
        {
            Identification =
                new ReaderIdentification(Identification.ReaderId, Identification.Username, @event.Email);
        }
    }
}