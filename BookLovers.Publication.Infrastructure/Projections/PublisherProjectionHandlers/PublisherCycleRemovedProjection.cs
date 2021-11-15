using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class PublisherCycleRemovedProjection :
        IProjectionHandler<PublisherCycleRemoved>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public PublisherCycleRemovedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherCycleRemoved @event)
        {
            var publisher = this._context.Publishers.Include(p => p.Cycles)
                .Single(p => p.Guid == @event.AggregateGuid);
            var publisherCycle = publisher.Cycles.Single(p => p.Guid == @event.CycleGuid);

            publisher.Cycles.Remove(publisherCycle);

            this._context.Publishers.AddOrUpdate(p => p.Id, publisher);

            this._context.SaveChanges();
        }
    }
}