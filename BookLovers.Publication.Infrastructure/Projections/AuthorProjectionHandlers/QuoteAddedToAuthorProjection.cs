using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class QuoteAddedToAuthorProjection :
        IProjectionHandler<QuoteAddedToAuthor>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteAddedToAuthorProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteAddedToAuthor @event)
        {
            var quoteQuery = this._context.Quotes.Where(p => p.Guid == @event.QuoteGuid)
                .FutureValue();
            var authorQuery = this._context.Authors.Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();

            var quote = quoteQuery.Value;
            var author = authorQuery.Value;

            quote.Author = author;

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}