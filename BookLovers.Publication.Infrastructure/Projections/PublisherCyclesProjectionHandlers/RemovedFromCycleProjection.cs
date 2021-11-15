using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherCyclesProjectionHandlers
{
    internal class RemovedFromCycleProjection :
        IProjectionHandler<BookRemovedFromCycle>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public RemovedFromCycleProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookRemovedFromCycle @event)
        {
            var publisherCycle = this._context.PublisherCycles.Include(p => p.CycleBooks)
                .Single(p => p.Guid == @event.AggregateGuid);
            var book = publisherCycle.CycleBooks.Single(p => p.Guid == @event.BookGuid);

            publisherCycle.CycleBooks.Remove(book);

            this._context.PublisherCycles.AddOrUpdate(p => p.Id, publisherCycle);

            this._context.SaveChanges();
        }
    }
}