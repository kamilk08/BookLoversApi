using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewArchivedProjection : IProjectionHandler<ReviewArchived>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewArchivedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReviewArchived @event)
        {
            this._context.Reviews.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReviewReadModel
                {
                    Status = @event.ReviewStatus
                });
        }
    }
}