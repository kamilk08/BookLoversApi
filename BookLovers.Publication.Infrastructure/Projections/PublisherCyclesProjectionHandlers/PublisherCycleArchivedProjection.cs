using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherCyclesProjectionHandlers
{
    internal class PublisherCycleArchivedProjection :
        IProjectionHandler<PublisherCycleArchived>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public PublisherCycleArchivedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherCycleArchived @event)
        {
            this._context.PublisherCycles.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new PublisherCycleReadModel
                {
                    Status = @event.Status
                });
        }
    }
}