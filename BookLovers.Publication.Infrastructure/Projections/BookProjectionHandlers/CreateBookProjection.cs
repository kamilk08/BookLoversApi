using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Services;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class CreateBookProjection : IProjectionHandler<BookCreated>, IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _contextAccessor;

        public CreateBookProjection(
            PublicationsContext context,
            IMapper mapper,
            ReadContextAccessor contextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._contextAccessor = contextAccessor;
        }

        public void Handle(BookCreated @event)
        {
            var authorsQuery = _context.Authors.Where(a => @event.BookAuthors.Contains(a.Guid)).Future();
            var publisherQuery = _context.Publishers.Where(p => p.Guid == @event.PublisherGuid).FutureValue();
            var seriesQuery = _context.Series.Where(p => p.Guid == @event.SeriesGuid).FutureValue();
            var readerQuery = _context.Readers.Where(p => p.ReaderGuid == @event.ReaderGuid).FutureValue();
            var cyclesQuery = _context.PublisherCycles.Where(pc => @event.Cycles.Contains(pc.Guid)).Future();

            var authors = authorsQuery.ToList();
            var publisher = publisherQuery.Value;
            var series = seriesQuery.Value;
            var reader = readerQuery.Value;
            var cycles = cyclesQuery.ToList();

            var book = _mapper.Map<BookCreated, BookReadModel>(@event);
            book.Authors = authors;
            book.Publisher = publisher;
            book.Series = series;
            book.Reader = reader;

            cycles.ForEach(c =>
            {
                c.CycleBooks.Add(book);
                _context.PublisherCycles.Attach(c);
            });
            _context.Books.Add(book);

            _context.SaveChanges();

            this._contextAccessor.AddReadModelId(@event.AggregateGuid, book.Id);
        }
    }
}