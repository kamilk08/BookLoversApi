using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class VisibleOnWall : ValueObject<VisibleOnWall>
    {
        public NotificationState NotificationState { get; }

        private VisibleOnWall()
        {
        }

        public VisibleOnWall(NotificationState notificationState)
        {
            NotificationState = notificationState;
        }

        public static VisibleOnWall Yes()
        {
            return new VisibleOnWall(NotificationState.Visible);
        }

        public static VisibleOnWall No()
        {
            return new VisibleOnWall(NotificationState.NotVisible);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + NotificationState.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(VisibleOnWall obj)
        {
            return NotificationState.Equals(obj.NotificationState);
        }
    }
}