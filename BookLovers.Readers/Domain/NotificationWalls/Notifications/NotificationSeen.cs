using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationSeen : ValueObject<NotificationSeen>
    {
        public bool SeenByReader { get; }

        public DateTime Date { get; }

        private NotificationSeen()
        {
        }

        public NotificationSeen(bool seen, DateTime date)
        {
            SeenByReader = seen;
            Date = date;
        }

        public static NotificationSeen Seen(DateTime date)
        {
            return new NotificationSeen(true, date);
        }

        public static NotificationSeen NotSeen()
        {
            return new NotificationSeen(false, default(DateTime));
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Date.GetHashCode();
            hash = (hash * 23) + SeenByReader.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(NotificationSeen obj)
        {
            return SeenByReader == obj.SeenByReader && Date == obj.Date;
        }
    }
}