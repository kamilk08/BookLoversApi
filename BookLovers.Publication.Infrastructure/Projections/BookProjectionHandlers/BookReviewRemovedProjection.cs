using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class BookReviewRemovedProjection :
        IProjectionHandler<BookReviewRemoved>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public BookReviewRemovedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookReviewRemoved @event)
        {
            var book = this._context.Books.Include(p => p.Reviews).Single(p => p.Guid == @event.AggregateGuid);
            var review = book.Reviews.Single(p => p.Guid == @event.ReviewGuid);

            book.Reviews.Remove(review);

            this._context.Reviews.Remove(review);

            this._context.Books.AddOrUpdate(p => p.Id, book);

            this._context.SaveChanges();
        }
    }
}