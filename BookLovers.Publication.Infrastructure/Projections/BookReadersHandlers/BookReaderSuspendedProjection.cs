using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.BookReaders;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookReadersHandlers
{
    internal class BookReaderSuspendedProjection :
        IProjectionHandler<BookReaderSuspended>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public BookReaderSuspendedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookReaderSuspended @event)
        {
            this._context.Readers.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReaderReadModel
                {
                    Status = @event.Status
                });
        }
    }
}