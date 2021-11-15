using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class BookHasNoSeriesProjection : IProjectionHandler<BookHasNoSeries>, IProjectionHandler
    {
        public void Handle(BookHasNoSeries @event)
        {
        }
    }
}