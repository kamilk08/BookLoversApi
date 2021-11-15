using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Entity;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers
{
    public class TimeLine : IEntityObject
    {
        internal IList<Activity> Activites = new List<Activity>();

        public Guid Guid { get; private set; }

        public IReadOnlyCollection<Activity> TimeLineActivities => Activites.ToList();

        private TimeLine()
        {
        }

        public TimeLine(Guid guid)
        {
            Guid = guid;
        }

        public bool HasActivity(Activity activity)
        {
            return Activites.Contains(activity);
        }

        public bool HasActivity(byte activityType, DateTime date)
        {
            return Activites.Any(p => p.Content.ActivityType.Value == (int) activityType && p.Content.Date == date);
        }

        public Activity GetActivity(Guid timeLineItemGuid, DateTime date, int activityTypeId)
        {
            return Activites.SingleOrDefault(p => p.TimeLineObjectGuid == timeLineItemGuid
                                                  && p.Content.ActivityType.Value == activityTypeId
                                                  && p.Content.Date.AreEqualWithoutTicks(date));
        }
    }
}