using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.NotificationWalls
{
    internal class NotificationHiddenProjection :
        IProjectionHandler<NotificationHiddenByReader>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NotificationHiddenProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NotificationHiddenByReader @event)
        {
            this._context.Notifications
                .Where(p => p.NotificationGuid == @event.NotificationGuid)
                .Update(p => new NotificationReadModel { IsVisible = false });
        }
    }
}