using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderSuspendedProjection : IProjectionHandler<ReaderSuspended>, IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderSuspendedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReaderSuspended @event)
        {
            this._context.Readers.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReaderReadModel
                {
                    Status = @event.ReaderStatus
                });
        }
    }
}