using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherCyclesProjectionHandlers
{
    internal class AddedToCycleProjection : IProjectionHandler<BookAddedToCycle>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AddedToCycleProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookAddedToCycle @event)
        {
            var cycleQuery = this._context.PublisherCycles.Include(p => p.CycleBooks)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var bookQuery = this._context.Books.Where(p => p.Guid == @event.BookGuid)
                .FutureValue();

            var publisherCycle = cycleQuery.Value;
            var book = bookQuery.Value;

            publisherCycle.CycleBooks.Add(book);

            this._context.PublisherCycles.AddOrUpdate(p => p.Id, publisherCycle);

            this._context.SaveChanges();
        }
    }
}