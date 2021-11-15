using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class ReviewSpoilerTagRemovedProjection :
        IProjectionHandler<ReviewSpoilerTagRemoved>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewSpoilerTagRemovedProjection(ReadersContext context) => this._context = context;

        public void Handle(ReviewSpoilerTagRemoved @event)
        {
            var reviewQuery = this._context.Reviews.Include(p => p.SpoilerTags)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var readerQuery = this._context.Readers.Where(p => p.Guid == @event.ReaderGuid)
                .FutureValue();

            var reader = readerQuery.Value;
            var reviewReadModel = reviewQuery.Value;

            var spoilerTag = reviewReadModel.SpoilerTags.Single(p => p.ReaderId == reader.ReaderId);

            reviewReadModel.SpoilerTags.Remove(spoilerTag);

            reviewReadModel.SpoilerTagsCount = reviewReadModel.SpoilerTags.Count;

            this._context.Reviews.AddOrUpdate(p => p.Id, reviewReadModel);

            this._context.ReviewSpoilerTags.Remove(spoilerTag);

            this._context.SaveChanges();
        }
    }
}