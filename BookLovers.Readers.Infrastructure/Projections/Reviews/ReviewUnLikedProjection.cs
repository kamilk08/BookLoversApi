using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewUnLikedProjection : IProjectionHandler<ReviewUnLiked>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewUnLikedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReviewUnLiked @event)
        {
            var reviewReadModel = this._context.Reviews.Include(p => p.Likes)
                .Single(p => p.Guid == @event.AggregateGuid);

            var like = reviewReadModel.Likes.Single(p => p.ReaderGuid == @event.LikeGiverGuid);

            reviewReadModel.Likes.Remove(like);
            reviewReadModel.LikesCount = @event.Likes;

            this._context.ReviewLikes.Remove(like);

            this._context.Reviews.AddOrUpdate(p => p.Id, reviewReadModel);

            this._context.SaveChanges();
        }
    }
}