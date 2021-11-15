using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorDetailsProjection :
        IProjectionHandler<AuthorDetailsChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorDetailsProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorDetailsChanged @event)
        {
            this._context.Authors
                .Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new AuthorReadModel
                {
                    BirthPlace = @event.BirthPlace,
                    BirthDate = @event.BirthDate,
                    DeathDate = @event.DeathDate
                });
        }
    }
}