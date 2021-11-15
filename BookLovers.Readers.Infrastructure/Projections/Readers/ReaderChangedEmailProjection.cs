using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderChangedEmailProjection :
        IProjectionHandler<ReaderEmailChanged>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderChangedEmailProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ReaderEmailChanged @event)
        {
            this._context.Readers.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new ReaderReadModel
                {
                    Email = @event.Email
                });
        }
    }
}