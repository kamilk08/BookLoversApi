using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class ChangeBookSeriesProjection : IProjectionHandler<SeriesChanged>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public ChangeBookSeriesProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(SeriesChanged @event)
        {
            var seriesQuery = _context.Series.Where(p => p.Guid == @event.SeriesGuid).FutureValue();
            var bookQuery = _context.Books.Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var series = seriesQuery.Value;
            var book = bookQuery.Value;

            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    SeriesId = series.Id,
                    PositionInSeries = @event.PositionInSeries
                });
        }
    }
}