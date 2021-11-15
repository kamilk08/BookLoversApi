using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorDescriptionProjection :
        IProjectionHandler<AuthorDescriptionChanged>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorDescriptionProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorDescriptionChanged @event)
        {
            this._context.Authors.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new AuthorReadModel
                {
                    DescriptionSource = @event.DescriptionSource,
                    AuthorWebsite = @event.AuthorWebsite,
                    AboutAuthor = @event.AboutAuthor
                });
        }
    }
}