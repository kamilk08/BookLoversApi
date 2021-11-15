using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class ShelfChangedProjection : IProjectionHandler<BookShelfChanged>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ShelfChangedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookShelfChanged @event)
        {
            var bookQuery = _context.Books.Where(p => p.BookGuid == @event.BookGuid)
                .FutureValue();
            var firstShelfQuery = _context.Shelves.Include(p => p.Books)
                .Where(p => p.Guid == @event.OldShelfGuid)
                .FutureValue();
            var secondShelfQuery = _context.Shelves.Include(p => p.Books)
                .Where(p => p.Guid == @event.NewShelfGuid)
                .FutureValue();

            var bookReadModel = bookQuery.Value;
            var firstShelf = firstShelfQuery.Value;
            var secondShelf = secondShelfQuery.Value;

            firstShelf.Books.Remove(bookReadModel);
            secondShelf.Books.Add(bookReadModel);

            _context.Shelves.Attach(firstShelf);
            _context.Shelves.Attach(secondShelf);

            _context.SaveChanges();
        }
    }
}