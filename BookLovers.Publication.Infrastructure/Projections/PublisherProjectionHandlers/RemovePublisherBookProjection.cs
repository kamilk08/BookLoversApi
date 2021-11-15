using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class RemovePublisherBookProjection :
        IProjectionHandler<PublisherBookRemoved>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public RemovePublisherBookProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherBookRemoved @event)
        {
            var publisherQuery = this._context.Publishers.Include(p => p.Books)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var bookQuery = this._context.Books.Where(p => p.Guid == @event.BookGuid)
                .FutureValue();

            var publisher = publisherQuery.Value;
            var bookReadModel = bookQuery.Value;

            publisher.Books.Remove(bookReadModel);

            this._context.Publishers.AddOrUpdate(p => p.Id, publisher);

            this._context.SaveChanges();
        }
    }
}