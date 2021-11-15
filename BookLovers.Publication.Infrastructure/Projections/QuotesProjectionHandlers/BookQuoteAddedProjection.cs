using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.QuotesProjectionHandlers
{
    internal class BookQuoteAddedProjection : IProjectionHandler<BookQuoteAdded>, IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _readContextAccessor;

        public BookQuoteAddedProjection(
            PublicationsContext context,
            IMapper mapper,
            ReadContextAccessor readContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._readContextAccessor = readContextAccessor;
        }

        public void Handle(BookQuoteAdded @event)
        {
            var reader = this._context.Readers.Single(p => p.ReaderGuid == @event.AddedBy);
            var book = this._context.Books.Single(p => p.Guid == @event.BookGuid);

            var quote = this._mapper.Map<BookQuoteAdded, QuoteReadModel>(@event);
            quote.ReaderId = reader.ReaderId;
            quote.ReaderGuid = reader.ReaderGuid;
            quote.Book = book;

            this._context.Quotes.Add(quote);

            this._context.SaveChanges();

            this._readContextAccessor.AddReadModelId(@event.AggregateGuid, quote.Id);
        }
    }
}