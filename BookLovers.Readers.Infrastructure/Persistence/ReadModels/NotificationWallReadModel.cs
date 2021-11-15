using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class NotificationWallReadModel : IReadModel<NotificationWallReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReaderId { get; set; }

        public IList<NotificationReadModel> Notifications { get; set; }

        public int Status { get; set; }

        public NotificationWallReadModel()
        {
            this.Notifications =
                new List<NotificationReadModel>();
        }
    }
}