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
    internal class ReviewReportedProjection : IProjectionHandler<ReviewReported>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReviewReportedProjection(ReadersContext context) => this._context = context;

        public void Handle(ReviewReported @event)
        {
            var reviewQuery = this._context.Reviews.Include(p => p.ReviewReports)
                .Where(p => p.Guid == @event.AggregateGuid).FutureValue();
            var readerQuery = this._context.Readers.Where(p => p.Guid == @event.ReportedByGuid).FutureValue();

            var review = reviewQuery.Value;
            var reader = readerQuery.Value;

            var reviewReportReadModel = new ReviewReportReadModel()
            {
                ReaderId = reader.ReaderId,
                ReviewId = review.Id
            };

            review.ReviewReports.Add(reviewReportReadModel);

            review.ReportsCount = review.ReviewReports.Count;

            this._context.Reviews.AddOrUpdate(p => p.Id, review);

            this._context.SaveChanges();
        }
    }
}