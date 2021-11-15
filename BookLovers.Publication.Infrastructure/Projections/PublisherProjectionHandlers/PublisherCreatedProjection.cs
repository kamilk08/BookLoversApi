using System.Data.Entity.Migrations;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherProjectionHandlers
{
    internal class PublisherCreatedProjection :
        IProjectionHandler<PublisherCreated>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly ReadContextAccessor _contextAccessor;

        public PublisherCreatedProjection(
            PublicationsContext context,
            ReadContextAccessor contextAccessor)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
        }

        public void Handle(PublisherCreated @event)
        {
            var publisher = new PublisherReadModel()
            {
                Guid = @event.AggregateGuid,
                Publisher = @event.Name,
                Status = @event.PublisherStatus
            };

            this._context.Publishers.AddOrUpdate(p => p.Id, publisher);

            this._context.SaveChanges();

            this._contextAccessor.AddReadModelId(@event.AggregateGuid, publisher.Id);
        }
    }
}