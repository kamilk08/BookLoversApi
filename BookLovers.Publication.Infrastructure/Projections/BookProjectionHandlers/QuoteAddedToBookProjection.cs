using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class QuoteAddedToBookProjection :
        IProjectionHandler<QuoteAddedToBook>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteAddedToBookProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteAddedToBook @event)
        {
            var quoteQuery = this._context.Quotes.Where(p => p.Guid == @event.QuoteGuid).FutureValue();
            var bookQuery = this._context.Books.Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var quote = quoteQuery.Value;
            var book = bookQuery.Value;

            quote.Book = book;

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}