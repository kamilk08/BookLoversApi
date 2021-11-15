using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationState : Enumeration
    {
        public static readonly NotificationState Visible = new NotificationState(1, nameof(Visible));
        public static readonly NotificationState NotVisible = new NotificationState(2, "Not visible");

        [JsonConstructor]
        protected NotificationState(int value, string name)
            : base(value, name)
        {
        }
    }
}