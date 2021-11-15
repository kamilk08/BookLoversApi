using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class NotificationReadModel
    {
        public int Id { get; set; }

        public Guid NotificationGuid { get; set; }

        public string NotificationObjects { get; set; }

        public int NotificationType { get; set; }

        public DateTime AppearedAt { get; set; }

        public bool IsVisible { get; set; }

        public DateTime? SeenAt { get; set; }
    }
}