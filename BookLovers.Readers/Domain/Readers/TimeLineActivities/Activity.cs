using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Readers.TimeLineActivities
{
    public class Activity : ValueObject<Activity>
    {
        public Guid TimeLineObjectGuid { get; }

        public ActivityContent Content { get; }

        public bool ShowToOthers { get; }

        private Activity()
        {
        }

        public Activity(Guid timeLineObjectGuid, ActivityContent content, bool showToOthers)
        {
            TimeLineObjectGuid = timeLineObjectGuid;
            Content = content;
            ShowToOthers = showToOthers;
        }

        public static Activity InitiallyPublic(
            Guid timeLineObjectGuid,
            ActivityContent content)
        {
            return new Activity(timeLineObjectGuid, content, true);
        }

        public static Activity InitiallyHidden(
            Guid timeLineObjectGuid,
            ActivityContent content)
        {
            return new Activity(timeLineObjectGuid, content, false);
        }

        public Activity Hide()
        {
            return new Activity(TimeLineObjectGuid, Content, false);
        }

        public Activity Show()
        {
            return new Activity(TimeLineObjectGuid, Content, true);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.TimeLineObjectGuid.GetHashCode();
            hash = (hash * 23) + this.ShowToOthers.GetHashCode();
            hash = (hash * 23) + this.Content.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Activity obj)
        {
            return TimeLineObjectGuid == obj.TimeLineObjectGuid
                   && Content == obj.Content && ShowToOthers == obj.ShowToOthers;
        }
    }
}