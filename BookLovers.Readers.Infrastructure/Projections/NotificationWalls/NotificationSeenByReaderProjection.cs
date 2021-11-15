using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.NotificationWalls
{
    internal class NotificationSeenByReaderProjection :
        IProjectionHandler<NotificationSeenByReader>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NotificationSeenByReaderProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NotificationSeenByReader @event)
        {
            this._context.Notifications
                .Where(p => p.NotificationGuid == @event.NotificationGuid)
                .Update(p => new NotificationReadModel
                {
                    IsVisible = true
                });
        }
    }
}