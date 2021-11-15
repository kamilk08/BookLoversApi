using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewLikedProjection : IProjectionHandler<ReviewLiked>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewLikedProjection(ReadersContext context) => this._context = context;

        public void Handle(ReviewLiked @event)
        {
            var readerQuery = this._context.Readers.Where(p => p.Guid == @event.LikeGiverGuid).FutureValue();
            var reviewQuery = this._context.Reviews.Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var reader = readerQuery.Value;
            var review = reviewQuery.Value;

            var reviewLike = new ReviewLikeReadModel()
            {
                ReaderId = reader.ReaderId,
                ReaderGuid = reader.Guid
            };

            review.Likes.Add(reviewLike);
            review.LikesCount = @event.Likes;

            this._context.Reviews.AddOrUpdate(p => p.Id, review);

            this._context.ReviewLikes.Add(reviewLike);

            this._context.SaveChanges();
        }
    }
}