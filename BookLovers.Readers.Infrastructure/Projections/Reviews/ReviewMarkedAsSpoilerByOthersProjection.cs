using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewMarkedAsSpoilerByOthersProjection :
        IProjectionHandler<ReviewMarkedByOtherReaders>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewMarkedAsSpoilerByOthersProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReviewMarkedByOtherReaders @event)
        {
            this._context.Reviews.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReviewReadModel
                {
                    MarkedAsSpoilerByOthers = @event.MarkedAsSpoilerByOthers
                });
        }
    }
}