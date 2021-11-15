using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Projections.NotificationWalls
{
    internal class NotificationCreatedProjection :
        IProjectionHandler<NotificationCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NotificationCreatedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NotificationCreated @event)
        {
            var notificationWallReadModel = this._context.NotificationWalls
                .Include(p => p.Notifications)
                .Single(p => p.Guid == @event.AggregateGuid);

            var notification = new NotificationReadModel()
            {
                NotificationGuid = @event.NotificationGuid,
                AppearedAt = @event.Date,
                NotificationObjects = JsonConvert.SerializeObject(@event.NotificationItems),
                SeenAt = null,
                IsVisible = @event.Visible,
                NotificationType = @event.NotificationSubTypeId
            };

            notificationWallReadModel.Notifications.Add(notification);

            this._context.Notifications.Add(notification);

            this._context.SaveChanges();
        }
    }
}