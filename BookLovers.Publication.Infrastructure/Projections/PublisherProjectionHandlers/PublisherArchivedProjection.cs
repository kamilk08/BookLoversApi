using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class PublisherArchivedProjection :
        IProjectionHandler<PublisherArchived>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public PublisherArchivedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(PublisherArchived @event)
        {
            this._context.Publishers.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new PublisherReadModel
                {
                    Status = @event.PublisherStatus
                });
        }
    }
}