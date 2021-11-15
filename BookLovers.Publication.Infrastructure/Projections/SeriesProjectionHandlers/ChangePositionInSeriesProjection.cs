using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.SeriesCycle;

namespace BookLovers.Publication.Infrastructure.Projections.SeriesProjectionHandlers
{
    internal class ChangePositionInSeriesProjection :
        IProjectionHandler<BookPositionInSeriesChanged>,
        IProjectionHandler
    {
        public void Handle(BookPositionInSeriesChanged @event)
        {
        }
    }
}