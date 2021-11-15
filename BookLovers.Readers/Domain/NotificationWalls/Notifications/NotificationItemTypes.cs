using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public static class NotificationItemTypes
    {
        private static IReadOnlyList<NotificationItemType> List =>
            typeof(NotificationItemType)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as NotificationItemType)
                .ToList();

        public static NotificationItemType Get(int notificationSubTypeId) =>
            List.SingleOrDefault(p => p.Value == notificationSubTypeId);
    }
}