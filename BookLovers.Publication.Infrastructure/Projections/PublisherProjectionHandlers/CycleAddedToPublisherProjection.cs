using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class CycleAddedToPublisherProjection :
        IProjectionHandler<PublisherCycleAdded>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public CycleAddedToPublisherProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherCycleAdded @event)
        {
            var publisherQuery = _context.Publishers.Include(p => p.Cycles)
                .Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();

            var cycleQuery = _context.PublisherCycles
                .Where(p => p.Guid == @event.CycleGuid)
                .FutureValue();

            var publisher = publisherQuery.Value;
            var cycle = cycleQuery.Value;

            publisher.Cycles.Add(cycle);

            _context.SaveChanges();
        }
    }
}