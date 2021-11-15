using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Readers.Domain.Readers.TimeLineActivities
{
    public class Activities
    {
        internal static readonly IList<ActivityType> Types = typeof(ActivityType)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(p => p.GetValue(p) as ActivityType).ToList();

        public static ActivityType GetActivity(int activityType) =>
            Types.SingleOrDefault(p => p.Value == activityType);

        public static bool IsActivityAvailable(int activityId) =>
            Types.Any(a => a.Value == activityId);
    }
}