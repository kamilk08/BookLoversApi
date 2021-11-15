using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Reviews
{
    internal class NewReviewSpoilerTagProjection :
        IProjectionHandler<NewReviewSpoilerTag>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public NewReviewSpoilerTagProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(NewReviewSpoilerTag @event)
        {
            var reviewQuery = this._context.Reviews
                .Include(p => p.SpoilerTags)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();

            var readerQuery = this._context.Readers
                .Where(p => p.Guid == @event.ReaderGuid)
                .FutureValue();

            var reviewReadModel = reviewQuery.Value;
            var readerReadModel = readerQuery.Value;

            var spoilerTag = new ReviewSpoilerTagReadModel()
            {
                ReviewId = reviewReadModel.Id,
                ReaderId = readerReadModel.ReaderId
            };

            reviewReadModel.SpoilerTags.Add(spoilerTag);
            reviewReadModel.SpoilerTagsCount = reviewReadModel.SpoilerTags.Count;

            this._context.ReviewSpoilerTags.Add(spoilerTag);

            this._context.Reviews.AddOrUpdate(p => p.Id, reviewReadModel);

            this._context.SaveChanges();
        }
    }
}