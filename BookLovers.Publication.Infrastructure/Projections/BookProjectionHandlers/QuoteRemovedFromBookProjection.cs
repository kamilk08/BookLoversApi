using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class QuoteRemovedFromBookProjection :
        IProjectionHandler<QuoteRemovedFromBook>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteRemovedFromBookProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteRemovedFromBook @event)
        {
            var quote = this._context.Quotes.Include(p => p.Book)
                .Single(p => p.Guid == @event.QuoteGuid);

            quote.Book = null;

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}