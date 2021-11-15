using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewMarkedAsSpoilerProjection :
        IProjectionHandler<ReviewMarkToggledByReader>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewMarkedAsSpoilerProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReviewMarkToggledByReader @event)
        {
            this._context.Reviews
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReviewReadModel
                {
                    MarkedAsSpoilerByReader = @event.MarkedAsASpoiler
                });
        }
    }
}