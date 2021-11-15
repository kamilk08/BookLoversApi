using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class AddedToBookcaseProjection : IProjectionHandler<BookAddedToShelf>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public AddedToBookcaseProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookAddedToShelf @event)
        {
            var bookQuery = _context.Books
                .Where(p => p.BookGuid == @event.BookGuid).FutureValue();

            var shelvesQuery = _context.Shelves.Include(p => p.Books)
                .Where(p => p.Guid == @event.ShelfGuid)
                .FutureValue();

            var book = bookQuery.Value;
            var shelves = shelvesQuery.Value;

            _context.Books.Attach(book);
            _context.Shelves.Attach(shelves);

            shelves.Books.Add(book);

            _context.SaveChanges();
        }
    }
}