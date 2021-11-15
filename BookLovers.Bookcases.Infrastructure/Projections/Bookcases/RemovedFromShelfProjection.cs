using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class RemovedFromBookcaseProjection :
        IProjectionHandler<BookRemovedFromShelf>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public RemovedFromBookcaseProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookRemovedFromShelf @event)
        {
            var shelfQuery = _context.Shelves.Include(p => p.Books).Where(p => p.Guid == @event.ShelfGuid)
                .FutureValue();
            var bookQuery = _context.Books.Where(p => p.BookGuid == @event.BookGuid).FutureValue();

            var shelf = shelfQuery.Value;
            var book = bookQuery.Value;

            shelf.Books.Remove(book);

            _context.SaveChanges();
        }
    }
}