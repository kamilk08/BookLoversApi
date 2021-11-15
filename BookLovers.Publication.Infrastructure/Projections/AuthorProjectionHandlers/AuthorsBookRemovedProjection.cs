using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorBookRemovedProjection :
        IProjectionHandler<AuthorBookRemoved>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorBookRemovedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorBookRemoved @event)
        {
            var authorQuery = this._context.Authors.Include(p => p.AuthorBooks)
                .Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();

            var bookQuery = this._context.Books
                .Where(p => p.Guid == @event.BookGuid).FutureValue();

            var author = authorQuery.Value;
            var book = bookQuery.Value;

            author.AuthorBooks.Remove(book);

            this._context.SaveChanges();
        }
    }
}