using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewEditedProjection : IProjectionHandler<ReviewEdited>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewEditedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReviewEdited @event)
        {
            var review = this._context.Reviews.Single(p => p.Guid == @event.AggregateGuid);

            this._context.Reviews.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReviewReadModel
                {
                    EditedDate = @event.EditedAt,
                    Review = @event.Review
                });

            this._context.EditedReviews.Add(new ReviewEditReadModel()
            {
                ReviewGuid = @event.AggregateGuid,
                EditedAt = @event.EditedAt,
                Review = @event.Review,
                ReviewId = review.Id
            });

            this._context.SaveChanges();
        }
    }
}