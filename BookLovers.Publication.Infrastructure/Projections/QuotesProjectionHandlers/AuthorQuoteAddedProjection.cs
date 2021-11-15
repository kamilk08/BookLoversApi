using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.QuotesProjectionHandlers
{
    internal class AuthorQuoteAddedProjection :
        IProjectionHandler<AuthorQuoteAdded>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _contextAccessor;

        public AuthorQuoteAddedProjection(
            PublicationsContext context,
            IMapper mapper,
            ReadContextAccessor contextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._contextAccessor = contextAccessor;
        }

        public void Handle(AuthorQuoteAdded @event)
        {
            var reader = this._context.Readers.Single(p => p.ReaderGuid == @event.AddedBy);

            var author = this._context.Authors.Single(p => p.Guid == @event.AuthorGuid);

            var quote = this._mapper.Map<AuthorQuoteAdded, QuoteReadModel>(@event);

            quote.ReaderId = reader.ReaderId;
            quote.ReaderGuid = reader.ReaderGuid;
            quote.Author = author;

            this._context.Quotes.Add(quote);

            this._context.SaveChanges();

            this._contextAccessor.AddReadModelId(@event.AggregateGuid, quote.Id);
        }
    }
}