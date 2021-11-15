using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.ShelfRecordTracker;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.ShelfRecordsTracker
{
    internal class ShelfRecordTrackerArchivedProjection :
        IProjectionHandler<ShelfRecordTrackerArchived>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ShelfRecordTrackerArchivedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(ShelfRecordTrackerArchived @event)
        {
            _context.ShelfRecordTrackers
                .Where(p => p.ShelfRecordTrackerGuid == @event.AggregateGuid)
                .Update(p => new ShelfRecordTrackerReadModel
                {
                    Status = AggregateStatus.Archived.Value
                });
        }
    }
}