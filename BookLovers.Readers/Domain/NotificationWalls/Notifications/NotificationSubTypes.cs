using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public static class NotificationSubTypes
    {
        private static IReadOnlyList<NotificationSubType> List =>
            typeof(NotificationSubType)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as NotificationSubType).ToList();

        public static NotificationSubType Get(int subTypeId) =>
            List.SingleOrDefault(p => p.Value == subTypeId);
    }
}