using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationItemType : Enumeration
    {
        public static readonly NotificationItemType Book = new NotificationItemType(1, nameof(Book));
        public static readonly NotificationItemType User = new NotificationItemType(2, nameof(User));
        public static readonly NotificationItemType Review = new NotificationItemType(3, nameof(Review));
        public static readonly NotificationItemType Librarian = new NotificationItemType(4, nameof(Librarian));
        public static readonly NotificationItemType Follower = new NotificationItemType(5, nameof(Follower));
        public static readonly NotificationItemType Author = new NotificationItemType(6, nameof(Author));

        [JsonConstructor]
        protected NotificationItemType(int value, string name)
            : base(value, name)
        {
        }
    }
}