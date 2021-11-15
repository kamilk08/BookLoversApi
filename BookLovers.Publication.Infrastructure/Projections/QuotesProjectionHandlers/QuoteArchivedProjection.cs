using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.QuotesProjectionHandlers
{
    internal class QuoteArchivedProjection : IProjectionHandler<QuoteArchived>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteArchivedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteArchived @event)
        {
            var quote = this._context.Quotes.Single(p => p.Guid == @event.AggregateGuid);

            quote.Status = @event.QuoteStatus;

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}