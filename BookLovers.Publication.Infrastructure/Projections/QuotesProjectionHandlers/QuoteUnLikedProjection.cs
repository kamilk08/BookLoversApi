using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.QuotesProjectionHandlers
{
    internal class QuoteUnLikedProjection : IProjectionHandler<QuoteUnLiked>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public QuoteUnLikedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(QuoteUnLiked @event)
        {
            var quoteQuery = this._context.Quotes.Include(p => p.QuoteLikes).Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();
            var readerQuery = this._context.Readers.Where(p => p.ReaderGuid == @event.UnlikedByGuid).FutureValue();

            var quote = quoteQuery.Value;
            var reader = readerQuery.Value;

            var like = quote.QuoteLikes.Single(p => p.ReaderId == reader.ReaderId);

            quote.QuoteLikes.Remove(like);

            this._context.QuoteLikes.Remove(like);

            this._context.Quotes.AddOrUpdate(p => p.Id, quote);

            this._context.SaveChanges();
        }
    }
}