using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationSubType : Enumeration
    {
        public static readonly NotificationSubType NewFollower =
            new NotificationSubType(NotificationType.Follower, 1, nameof(NewFollower));

        public static readonly NotificationSubType LostFollower =
            new NotificationSubType(NotificationType.Follower, 2, nameof(LostFollower));

        public static readonly NotificationSubType BookAcceptedByLibrarian =
            new NotificationSubType(NotificationType.Book, 3, nameof(BookAcceptedByLibrarian));

        public static readonly NotificationSubType BookDismissedByLibrarian =
            new NotificationSubType(NotificationType.Book, 4, nameof(BookDismissedByLibrarian));

        public static readonly NotificationSubType ReviewLiked =
            new NotificationSubType(NotificationType.Review, 5, nameof(ReviewLiked));

        public static readonly NotificationSubType ReviewReported =
            new NotificationSubType(NotificationType.Review, 6, nameof(ReviewReported));

        public static readonly NotificationSubType AuthorAcceptedByLibrarian =
            new NotificationSubType(NotificationType.Author, 7, nameof(AuthorAcceptedByLibrarian));

        public static readonly NotificationSubType AuthorDismissedByLibrarian =
            new NotificationSubType(NotificationType.Author, 8, nameof(AuthorDismissedByLibrarian));

        internal NotificationType NotificationType { get; }

        [JsonConstructor]
        protected NotificationSubType(NotificationType notificationType, int value, string name)
            : base(value, name)
        {
            NotificationType = notificationType;
        }
    }
}