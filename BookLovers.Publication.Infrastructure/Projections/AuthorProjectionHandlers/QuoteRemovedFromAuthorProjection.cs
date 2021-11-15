using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class QuoteRemovedFromAuthorProjection :
        IProjectionHandler<QuoteRemovedFromAuthor>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteRemovedFromAuthorProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteRemovedFromAuthor @event)
        {
            var quote = this._context.Quotes.Include(p => p.Author)
                .Single(p => p.Guid == @event.QuoteGuid);

            quote.Author = null;

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}