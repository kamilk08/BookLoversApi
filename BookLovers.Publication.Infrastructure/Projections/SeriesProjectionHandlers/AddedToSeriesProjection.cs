using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.SeriesProjectionHandlers
{
    internal class AddedToSeriesProjection : IProjectionHandler<AddedToSeries>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AddedToSeriesProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AddedToSeries @event)
        {
            var seriesQuery = this._context.Series
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var bookQuery = this._context.Books.Include(p => p.Series)
                .Where(p => p.Guid == @event.BookGuid)
                .FutureValue();

            var series = seriesQuery.Value;
            var book = bookQuery.Value;

            series.Books.Add(book);

            this._context.Series.AddOrUpdate(p => p.Id, series);

            this._context.SaveChanges();
        }
    }
}