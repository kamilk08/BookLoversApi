using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.BookProjectionHandlers
{
    internal class ChangeBookPublisherProjection :
        IProjectionHandler<PublisherChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public ChangeBookPublisherProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherChanged @event)
        {
            var publisher = _context.Publishers.Single(p => p.Guid == @event.PublisherGuid);

            _context.Books.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookReadModel
                {
                    PublisherId = publisher.Id
                });
        }
    }
}