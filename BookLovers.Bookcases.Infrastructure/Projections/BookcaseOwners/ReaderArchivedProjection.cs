using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Readers;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.BookcaseOwners
{
    internal class ReaderArchivedProjection :
        IProjectionHandler<BookcaseOwnerArchived>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ReaderArchivedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookcaseOwnerArchived @event)
        {
            _context.Readers
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReaderReadModel
                    { Status = AggregateStatus.Archived.Value });
        }
    }
}