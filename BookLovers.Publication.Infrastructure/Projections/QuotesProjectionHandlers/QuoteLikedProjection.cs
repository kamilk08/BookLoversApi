using System.Data.Entity;
using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.QuotesProjectionHandlers
{
    internal class QuoteLikedProjection : IProjectionHandler<QuoteLiked>, IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public QuoteLikedProjection(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Handle(QuoteLiked @event)
        {
            var quoteQuery = this._context.Quotes.Include(p => p.QuoteLikes).Where(p => p.Guid == @event.AggregateGuid)
                .FutureValue();
            var readerQuery = this._context.Readers.Where(p => p.ReaderGuid == @event.AddedBy).FutureValue();
            var quote = quoteQuery.Value;
            var reader = readerQuery.Value;

            var like = this._mapper.Map<QuoteLiked, QuoteLikeReadModel>(@event);
            like.ReaderId = reader.ReaderId;
            like.ReaderGuid = reader.ReaderGuid;

            this._context.QuoteLikes.Add(like);

            quote.QuoteLikes.Add(like);

            this._context.SaveChanges();
        }
    }
}