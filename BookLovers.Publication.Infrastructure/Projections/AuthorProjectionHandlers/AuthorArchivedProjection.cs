using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorArchivedProjection : IProjectionHandler<AuthorArchived>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorArchivedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorArchived @event)
        {
            this._context.Authors.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new AuthorReadModel
                {
                    Status = @event.AuthorStatus
                });
        }
    }
}