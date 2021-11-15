using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class RemoveBookAuthorProjection : IProjectionHandler<AuthorRemoved>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public RemoveBookAuthorProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorRemoved @event)
        {
            var authorQuery = this._context.Authors.Where(p => p.Guid == @event.AuthorGuid).FutureValue();
            var bookQuery = this._context.Books.Include(p => p.Authors)
                .Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();

            var author = authorQuery.Value;
            var book = bookQuery.Value;

            book.Authors.Remove(author);

            this._context.SaveChanges();
        }
    }
}