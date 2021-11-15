using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.SeriesProjectionHandlers
{
    internal class SeriesArchivedProjection : IProjectionHandler<SeriesArchived>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public SeriesArchivedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(SeriesArchived @event)
        {
            _context.Series
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new SeriesReadModel
                {
                    Status = @event.SeriesStatus
                });
        }
    }
}