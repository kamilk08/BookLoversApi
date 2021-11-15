using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.NotificationWalls
{
    internal class NotificationWallArchivedProjection :
        IProjectionHandler<NotificationWallArchived>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NotificationWallArchivedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NotificationWallArchived @event)
        {
            this._context.NotificationWalls
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new NotificationWallReadModel
                {
                    Status = @event.Status
                });
        }
    }
}