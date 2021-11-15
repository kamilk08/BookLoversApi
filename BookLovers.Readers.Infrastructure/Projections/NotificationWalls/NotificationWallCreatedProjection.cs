using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.NotificationWalls
{
    internal class NotificationWallCreatedProjection :
        IProjectionHandler<NotificationWallCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NotificationWallCreatedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NotificationWallCreated @event)
        {
            var reader = this._context.Readers.Single(p => p.Guid == @event.ReaderGuid);

            this._context.NotificationWalls.Add(new NotificationWallReadModel()
            {
                Guid = @event.AggregateGuid,
                ReaderId = reader.ReaderId,
                Status = @event.Status
            });

            this._context.SaveChanges();
        }
    }
}