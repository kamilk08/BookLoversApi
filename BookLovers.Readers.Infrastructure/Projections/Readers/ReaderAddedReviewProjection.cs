using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderAddedReviewProjection :
        IProjectionHandler<ReaderAddedReview>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderAddedReviewProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReaderAddedReview @event)
        {
            var reader = this._context.Readers.Include(p => p.AddedResources)
                .Single(p => p.Guid == @event.AggregateGuid);

            var resource = new AddedResourceReadModel()
            {
                ResourceGuid = @event.ReviewGuid
            };

            reader.AddedResources.Add(resource);

            this._context.Readers.AddOrUpdate(p => p.Id, reader);

            this._context.SaveChanges();
        }
    }
}