using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class AddedResourceRemovedProjection :
        IProjectionHandler<AddedResourceRemoved>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public AddedResourceRemovedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(AddedResourceRemoved @event)
        {
            var resource = this._context.AddedResources.Single(p =>
                p.ResourceGuid == @event.ResourceGuid);

            this._context.AddedResources.Remove(resource);

            this._context.SaveChanges();
        }
    }
}