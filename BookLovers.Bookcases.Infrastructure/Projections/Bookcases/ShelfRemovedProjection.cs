using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class ShelfRemovedProjection : IProjectionHandler<ShelfRemoved>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ShelfRemovedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(ShelfRemoved @event)
        {
            var bookcaseQuery = _context.Bookcases.Include(p => p.Shelves)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var shelfQuery = _context.Shelves
                .Where(p => p.Guid == @event.ShelfGuid).FutureValue();

            var bookcase = bookcaseQuery.Value;
            var shelf = shelfQuery.Value;

            _context.Shelves.Remove(shelf);

            bookcase.Shelves.Remove(shelf);

            _context.SaveChanges();
        }
    }
}