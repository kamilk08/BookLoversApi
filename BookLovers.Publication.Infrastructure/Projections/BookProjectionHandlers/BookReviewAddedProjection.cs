using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class BookReviewAddedProjection : IProjectionHandler<BookReviewAdded>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public BookReviewAddedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookReviewAdded @event)
        {
            var booksQuery = this._context.Books.Include(p => p.Reviews)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var readersQuery = this._context.Readers
                .Where(p => p.ReaderGuid == @event.ReaderGuid).FutureValue();

            var book = booksQuery.Value;
            var reader = readersQuery.Value;

            var review = new ReviewReadModel()
            {
                Guid = @event.ReviewGuid,
                ReaderId = reader.Id
            };

            book.Reviews.Add(review);
            this._context.Reviews.Add(review);

            this._context.SaveChanges();
        }
    }
}