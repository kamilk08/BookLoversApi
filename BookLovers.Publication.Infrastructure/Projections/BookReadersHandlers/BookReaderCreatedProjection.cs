using System.Data.Entity.Migrations;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.BookReaders;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Projections.BookReadersHandlers
{
    internal class BookReaderCreatedProjection :
        IProjectionHandler<BookReaderCreated>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public BookReaderCreatedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookReaderCreated @event)
        {
            this._context.Readers.AddOrUpdate(
                p => p.ReaderGuid,
                new ReaderReadModel()
                {
                    Guid = @event.AggregateGuid,
                    ReaderGuid = @event.BookReaderGuid,
                    ReaderId = @event.BookReaderId,
                    Status = @event.Status
                });

            this._context.SaveChanges();
        }
    }
}