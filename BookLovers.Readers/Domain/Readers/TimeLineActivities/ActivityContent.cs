using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Readers.TimeLineActivities
{
    public class ActivityContent : ValueObject<ActivityContent>
    {
        public string Title { get; }

        public DateTime Date { get; }

        public ActivityType ActivityType { get; }

        private ActivityContent()
        {
        }

        public ActivityContent(string title, DateTime date, ActivityType activityType)
        {
            Title = title;
            Date = date;
            ActivityType = activityType;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Date.GetHashCode();
            hash = (hash * 23) + this.Title.GetHashCode();
            hash = (hash * 23) + this.ActivityType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ActivityContent obj) =>
            Title == obj.Title && ActivityType.Equals(obj.ActivityType)
                               && Date == obj.Date;
    }
}