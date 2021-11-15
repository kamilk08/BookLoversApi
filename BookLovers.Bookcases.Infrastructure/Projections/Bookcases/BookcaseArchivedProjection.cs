using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class BookcaseArchivedProjection :
        IProjectionHandler<BookcaseArchived>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookcaseArchivedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookcaseArchived @event)
        {
            _context.Bookcases
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new BookcaseReadModel
                {
                    Status = AggregateStatus.Archived.Value
                });
        }
    }
}