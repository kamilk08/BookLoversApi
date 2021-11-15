using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationType : Enumeration
    {
        public static readonly NotificationType Follower = new NotificationType(1, nameof(Follower));
        public static readonly NotificationType Book = new NotificationType(3, nameof(Book));
        public static readonly NotificationType Author = new NotificationType(4, nameof(Author));
        public static readonly NotificationType Review = new NotificationType(5, nameof(Review));

        private static readonly IReadOnlyList<NotificationType>
            AvailableNotifications = typeof(NotificationType)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as NotificationType)
                .ToList();

        [JsonConstructor]
        protected NotificationType(int value, string name)
            : base(value, name)
        {
        }

        public static NotificationType Get(int type) =>
            AvailableNotifications.SingleOrDefault(p => p.Value == type);
    }
}