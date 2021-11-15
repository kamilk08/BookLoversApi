using System.Data.Entity.Migrations;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.ShelfRecordTracker;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.ShelfRecordsTracker
{
    internal class ShelfRecordTrackerCreatedProjection :
        IProjectionHandler<ShelfRecordTrackerCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ShelfRecordTrackerCreatedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(ShelfRecordTrackerCreated @event)
        {
            _context.ShelfRecordTrackers.AddOrUpdate(
                p => p.Id,
                new ShelfRecordTrackerReadModel
                {
                    BookcaseGuid = @event.BookcaseGuid,
                    ShelfRecordTrackerGuid = @event.AggregateGuid,
                    Status = @event.Status
                });

            _context.SaveChanges();
        }
    }
}