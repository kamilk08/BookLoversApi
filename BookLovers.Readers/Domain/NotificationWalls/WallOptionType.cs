using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.NotificationWalls
{
    public class WallOptionType : Enumeration
    {
        public static readonly WallOptionType HideNotification = new WallOptionType(1, "Hide new notification");

        public static readonly WallOptionType HideNotificationFromOther =
            new WallOptionType(2, "Hide notification from other");

        public static readonly WallOptionType HideNotificationAboutBook =
            new WallOptionType(3, "Hide notification about book");

        public static readonly WallOptionType HideNotificationAboutReview =
            new WallOptionType(4, "Hide notification about review");

        [JsonConstructor]
        protected WallOptionType(byte value, string name)
            : base(value, name)
        {
        }
    }
}