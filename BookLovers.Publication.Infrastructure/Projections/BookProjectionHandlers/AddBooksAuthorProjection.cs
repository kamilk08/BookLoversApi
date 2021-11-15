using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class AddBooksAuthorProjection : IProjectionHandler<AuthorAdded>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AddBooksAuthorProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorAdded @event)
        {
            var authorQuery = this._context.Authors.Where(p => p.Guid == @event.AuthorGuid)
                .FutureValue();
            var bookQuery = this._context.Books.Include(p => p.Authors)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var author = authorQuery.Value;
            var bookReadModel = bookQuery.Value;

            this._context.Authors.Attach(author);
            bookReadModel.Authors.Add(author);

            this._context.SaveChanges();
        }
    }
}