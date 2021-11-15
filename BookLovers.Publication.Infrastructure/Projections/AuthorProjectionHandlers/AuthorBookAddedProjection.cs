using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorBookAddedProjection : IProjectionHandler<AuthorBookAdded>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorBookAddedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorBookAdded @event)
        {
            var booksQuery = this._context.Books
                .Where(p => p.Guid == @event.BookGuid).FutureValue();
            var authorsQuery = this._context.Authors.Include(p => p.AuthorBooks)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var book = booksQuery.Value;
            var author = authorsQuery.Value;

            this._context.Books.Attach(book);

            author.AuthorBooks.Add(book);

            this._context.SaveChanges();
        }
    }
}