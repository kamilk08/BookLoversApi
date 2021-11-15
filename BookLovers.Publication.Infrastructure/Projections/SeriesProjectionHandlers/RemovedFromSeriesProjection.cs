using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.SeriesProjectionHandlers
{
    internal class RemovedFromSeriesProjection :
        IProjectionHandler<BookRemovedFromSeries>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public RemovedFromSeriesProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(BookRemovedFromSeries @event)
        {
            _context.Books.Include(p => p.Series)
                .Where(p => p.Guid == @event.BookGuid)
                .Update(p => new BookReadModel
                {
                    SeriesId = null,
                    PositionInSeries = null,
                });
        }
    }
}