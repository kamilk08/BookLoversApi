using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class PublisherBookAddedProjection :
        IProjectionHandler<PublisherBookAdded>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public PublisherBookAddedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherBookAdded @event)
        {
            var publisherQuery = this._context.Publishers.Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var bookQuery = this._context.Books.Where(p => p.Guid == @event.BookGuid).FutureValue();

            var publisher = publisherQuery.Value;
            var book = bookQuery.Value;

            this._context.Books.Attach(book);

            publisher.Books.Add(book);

            this._context.SaveChanges();
        }
    }
}