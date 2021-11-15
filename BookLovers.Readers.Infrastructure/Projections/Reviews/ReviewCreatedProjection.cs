using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Services;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewCreatedProjection : IProjectionHandler<ReviewCreated>, IProjectionHandler
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _readContextAccessor;

        public ReviewCreatedProjection(
            ReadersContext context,
            IMapper mapper,
            ReadContextAccessor readContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._readContextAccessor = readContextAccessor;
        }

        public void Handle(ReviewCreated @event)
        {
            var readerQuery = this._context.Readers.Where(p => p.Guid == @event.ReaderGuid).FutureValue();
            var bookQuery = this._context.Books.Where(p => p.BookGuid == @event.BookGuid).FutureValue();

            var reader = readerQuery.Value;
            var book = bookQuery.Value;

            var review = this._mapper.Map<ReviewCreated, ReviewReadModel>(@event);
            review.Reader = reader;
            review.Book = book;

            this._context.Reviews.Add(review);

            this._context.SaveChanges();

            this._readContextAccessor.AddReadModelId(review.Guid, review.Id);
        }
    }
}